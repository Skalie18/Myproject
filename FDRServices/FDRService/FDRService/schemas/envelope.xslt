<?xml version="1.0"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:crs="CRS610MI" exclude-result-prefixes="crs">
<xsl:output method="xml"/>
<xsl:template match="/">
  <soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/" soap:encodingStyle="http://schemas.xmlsoap.org/soap/encoding/">
    <soap:Body>
    <xsl:apply-templates select="node()|@*"/>
    </soap:Body>
  </soap:Envelope>
</xsl:template>
<xsl:template match="node()|@*">
    <xsl:copy>
        <xsl:apply-templates select="node()|@*"/>
    </xsl:copy>
</xsl:template>
</xsl:stylesheet>