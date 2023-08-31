using net.liberty_development.SaxonHE11s9apiExtensions;
using net.sf.saxon.s9api;

var xml = """
          <root>
            <root>
                <order>131as</order>
                <type>se134</type>
                <num>15643</num>
                <curr>USD</curr>
                <code>{"desc":"Testing123","city":"Dubai","auth":"author","store":[{"quantity":1,"amount":2.00},{"quantity":100,"amount":-8.33},{"quantity":300,"amount":-15.00}]}</code>
            </root>
            <root>
                <order>231as</order>
                <type>se134</type>
                <num>15643</num>
                <curr>AED</curr>
                <code>{"desc":"testdesc","city":"SHarjah","auth":"Mohammad","amount":20.00,"shop":"test","store":[{"quantity":1,"amount":10.00},{"quantity":100,"amount":98.33},{"quantity":300,"amount":-1.00}]}</code>
            </root>
            <root>
                <order>331as</order>
                <type>se134</type>
                <num>15643</num>
                <curr>SAR</curr>
                <code/>
            </root>
            <root>
                <order>431as</order>
                <type>se134</type>
                <num>15643</num>
                <curr>USD</curr>
                <code/>
            </root>
            <root>
                <order>531as</order>
                <type>Tse134</type>
                <num>15643</num>
                <curr>AED</curr>
                <code>{"desc":"testdesc","city":"Abudabhi","auth":"Mr.121","amount":10.00,"shop":"testa","store":[{"quantity":71,"amount":10.00},{"quantity":100,"amount":95.33},{"quantity":300,"amount":-8.00}]}</code>
            </root>
          </root>
          """;

var xslt = """
           <xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
           	xmlns:xs="http://www.w3.org/2001/XMLSchema"
           	xmlns:fn="http://www.w3.org/2005/xpath-functions"
           	exclude-result-prefixes="#all"
           	expand-text="yes"
           	version="3.0">
           
             <xsl:mode on-no-match="shallow-copy"/>
           
             <xsl:output method="xml" indent="yes"/>
           
             <xsl:template match="code[parse-json(.) instance of map(*)]">
               <xsl:copy>
                 <xsl:apply-templates select="json-to-xml(.)!* => sort((), function($el) { $el/@key})" mode="json"/>
               </xsl:copy>
             </xsl:template>
             
             <xsl:mode name="json" on-no-match="shallow-copy"/>
             
             <xsl:template mode="json" match="fn:*[@key]">
               <xsl:element name="{@key}">
                 <xsl:apply-templates mode="#current"/>
               </xsl:element>
             </xsl:template>
             
             <xsl:template mode="json" match="fn:array[@key]/fn:map[not(@key)]">
               <element>
                 <xsl:apply-templates select="* => sort((), function($el) { $el/@key })" mode="#current"/>
               </element>
             </xsl:template>
             
             <xsl:template mode="json" match="/fn:map[not(@key)]">
               <root>
                 <xsl:apply-templates select="* => sort((), function($el) { $el/@key })" mode="#current"/>
               </root>
             </xsl:template>
             
           </xsl:stylesheet>
           """;

var processor = new Processor(false);

var xsltCompiler = processor.newXsltCompiler();

var xsltExecutable = xsltCompiler.compile(xslt.AsSource());

var xslt30Transformer = xsltExecutable.load30();

xslt30Transformer.transform(xml.AsSource(), processor.NewSerializer(Console.Out));

