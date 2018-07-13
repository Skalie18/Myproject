using System;
using System.Diagnostics;
using IBM.XMS;

namespace FRD_MNE_Services
{
    public static class FdrMessaging
    {
        private static void RespondToCbc(string qMessage, string correlationId)
        {
            try
            {
                var xmsFactoryInstance = XMSFactoryFactory.GetInstance(XMSC.CT_WMQ);
                var connectionFactory = xmsFactoryInstance.CreateConnectionFactory();
                connectionFactory.SetStringProperty(XMSC.WMQ_HOST_NAME,
                    MQConfigurationSettings.MNE_Equire_OUT_QConnection);
                connectionFactory.SetIntProperty(XMSC.WMQ_PORT, MQConfigurationSettings.MNE_Equire_OUT_PortNumber);
                connectionFactory.SetStringProperty(XMSC.WMQ_CHANNEL,
                    MQConfigurationSettings.MNE_Equire_OUT_QChannelName);
                connectionFactory.SetIntProperty(XMSC.WMQ_CONNECTION_MODE, XMSC.WMQ_CM_CLIENT);
                connectionFactory.SetStringProperty(XMSC.WMQ_QUEUE_MANAGER, string.Empty);
                connectionFactory.SetIntProperty(XMSC.WMQ_BROKER_VERSION, XMSC.WMQ_BROKER_V1);

                //_connectionFactory.SetStringProperty(XMSC.WMQ_MESSAGE_BODY, XMSC.messa);

                var connection = connectionFactory.CreateConnection();
                var connectionSession = connection.CreateSession(false, AcknowledgeMode.AutoAcknowledge);
                var destination =
                    connectionSession.CreateQueue(string.Format("queue://{0}/{1}", string.Empty, MQConfigurationSettings.CBC_Declaration_OUT_QName));

                destination.SetIntProperty(XMSC.DELIVERY_MODE, MQConfigurationSettings.DELIVERY_MODE);
                destination.SetIntProperty(XMSC.WMQ_TARGET_CLIENT, XMSC.WMQ_TARGET_DEST_MQ);
                destination.SetBooleanProperty(XMSC.WMQ_MQMD_READ_ENABLED, true);
                destination.SetBooleanProperty(XMSC.WMQ_MQMD_WRITE_ENABLED, true);

                var producer = connectionSession.CreateProducer(destination);

                var textMessage = connectionSession.CreateTextMessage(qMessage);
                textMessage.SetIntProperty(XMSC.JMS_IBM_ENCODING, MQConfigurationSettings.JMS_IBM_ENCODING);
                textMessage.SetIntProperty(XMSC.JMS_IBM_CHARACTER_SET, MQConfigurationSettings.JMS_IBM_CHARACTER_SET);
                textMessage.JMSCorrelationID = correlationId;
                producer.Send(textMessage);
                producer.Close();
                producer.Dispose();
                destination.Dispose();
                connectionSession.Close();
                connectionSession.Dispose();
            }
            catch (Exception exception)
            {
                FdrCommon.LogEvent(exception, EventLogEntryType.Error);
            }
        }

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

                        destination.SetIntProperty(XMSC.DELIVERY_MODE, MQConfigurationSettings.DELIVERY_MODE);
                        destination.SetIntProperty(XMSC.WMQ_TARGET_CLIENT, XMSC.WMQ_TARGET_DEST_MQ);
                        destination.SetBooleanProperty(XMSC.WMQ_MQMD_READ_ENABLED, true);
                        destination.SetBooleanProperty(XMSC.WMQ_MQMD_WRITE_ENABLED, true);

                        using (var producer = connectionSession.CreateProducer(destination))
                        {
                            var textMessage = connectionSession.CreateTextMessage(queueInfo.Message);
                            textMessage.SetIntProperty(XMSC.JMS_IBM_ENCODING, MQConfigurationSettings.JMS_IBM_ENCODING);
                            textMessage.SetIntProperty(XMSC.JMS_IBM_CHARACTER_SET,
                                MQConfigurationSettings.JMS_IBM_CHARACTER_SET);
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

            catch (Exception exception)
            {
                FdrCommon.LogEvent(exception, EventLogEntryType.Error);
            }
        }
    }
}
