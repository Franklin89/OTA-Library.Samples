using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using MLSoftware.OTA;
using ReadReservations;

namespace ConsoleApp3
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new OTA2009A_ReadReservationsSoapClient(OTA2009A_ReadReservationsSoapClient.EndpointConfiguration.OTA2009A_ReadReservationsSoap);
            client.Open();

            var readRequest = new OTA_ReadRQ
            {
                Version = 1,
                SchemaLocation = "http://www.opentravel.org/OTA/2003/05 OTA_ReadRQ.xsd",
                EchoToken = "34267",
                TimeStamp= DateTime.Now,
                Target = MessageAcknowledgementTypeTarget.Test,
                POS = new System.Collections.Generic.List<SourceType>
                {
                    new SourceType
                    {
                        AgentDutyCode = "FERATEL"
                    }
                },
                ReadRequests = new OTA_ReadRQReadRequests
                {
                    Items = new System.Collections.Generic.List<object>
                    {
                        new OTA_ReadRQReadRequestsHotelReadRequest
                        {
                            HotelCode = "SIMSIM",
                            SelectionCriteria = new System.Collections.Generic.List<OTA_ReadRQReadRequestsHotelReadRequestSelectionCriteria>
                            {
                                new OTA_ReadRQReadRequestsHotelReadRequestSelectionCriteria
                                {
                                    DateType  = OTA_ReadRQReadRequestsHotelReadRequestSelectionCriteriaDateType.LastUpdateDate,
                                    Start = "2017-04-01",
                                    End = "2017-04-01",
                                }
                            }
                        }
                    }
                }
            };

            var serializer = new XmlSerializer(typeof(OTA_ReadRQ));
            var encoding = Encoding.ASCII;
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings
            {
                Indent = false,
                OmitXmlDeclaration = true,
                Encoding = encoding
            };

            using (var stream = new MemoryStream())
            {
                using (var xmlWriter = XmlWriter.Create(stream, xmlWriterSettings))
                {
                    serializer.Serialize(xmlWriter, readRequest);
                }
                
                var response = client.GetResponseAsync(encoding.GetString(stream.ToArray())).Result;
                Console.WriteLine(response.Body.GetResponseResult);
                Console.ReadKey();
            }

            client.Close();
        }
    }
}
