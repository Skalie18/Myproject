using System;
using System.Diagnostics;
using IBM.XMS;


    public static class FdrMessaging
    {
        public static void SendMessageToQueue(QueueInfo queueInfo)
        {
            try
            {
                var xmsFactoryInstance = XMSFactoryFactory.GetInstance(XMSC.CT_WMQ);
                var connectionFactory = xmsFactoryInstance.CreateConnectionFactory();
                connectionFactory.SetStringProperty(XMSC.WMQ_HOST_NAME, queueInfo.HostName);
                connectionFactory.SetIntProperty(XMSC.WMQ_PORT, queueInfo.Port);
                connectionFactory.SetStringProperty(XMSC.WMQ_CHANNEL, queueInfo.Channel);
                connectionFactory.SetIntProperty(XMSC.WMQ_CONNECTION_MODE, XMSC.WMQ_CM_CLIENT);
                connectionFactory.SetStringProperty(XMSC.WMQ_QUEUE_MANAGER, queueInfo.NoManager);
                connectionFactory.SetIntProperty(XMSC.WMQ_BROKER_VERSION, XMSC.WMQ_BROKER_V1);
                var connection = connectionFactory.CreateConnection();
                using (var connectionSession = connection.CreateSession(false, AcknowledgeMode.AutoAcknowledge))
                {
                    using (var destination = connectionSession.CreateQueue(queueInfo.QueueConnection))
                    {

                        destination.SetIntProperty(XMSC.DELIVERY_MODE, MQConfigurationSettings.DeliveryMode);
                        destination.SetIntProperty(XMSC.WMQ_TARGET_CLIENT, XMSC.WMQ_TARGET_DEST_MQ);
                        destination.SetBooleanProperty(XMSC.WMQ_MQMD_READ_ENABLED, true);
                        destination.SetBooleanProperty(XMSC.WMQ_MQMD_WRITE_ENABLED, true);

                        using (var producer = connectionSession.CreateProducer(destination))
                        {
                            var textMessage = connectionSession.CreateTextMessage(queueInfo.Message);
                            textMessage.SetIntProperty(XMSC.JMS_IBM_ENCODING, MQConfigurationSettings.JmsIbmEncoding);
                            textMessage.SetIntProperty(XMSC.JMS_IBM_CHARACTER_SET,
                                MQConfigurationSettings.JmsIbmCharacterSet);
                            if (!string.IsNullOrEmpty(queueInfo.CorrelationId))
                            {
                                textMessage.JMSCorrelationID = queueInfo.CorrelationId;
                            }
                            producer.Send(textMessage);
                            producer.Close();
                            connectionSession.Close();
                            connectionSession.Dispose();
                        }
                    }
                }
            }

            catch (Exception)
            {
                //FdrCommon.LogEvent(exception, EventLogEntryType.Error);
            }
        }
    }
