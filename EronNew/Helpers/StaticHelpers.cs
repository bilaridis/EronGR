using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace EronNew.Helpers
{
    /// <summary>
    /// Reference Article http://www.codeproject.com/KB/tips/SerializedObjectCloner.aspx
    /// Provides a method for performing a deep copy of an object.
    /// Binary Serialization is used to perform the copy.
    /// </summary>
    public static class ObjectCopier
    {
        /// <summary>
        /// Perform a deep copy of the object via serialization.
        /// </summary>
        /// <typeparam name="T">The type of object being copied.</typeparam>
        /// <param name="source">The object instance to copy.</param>
        /// <returns>A deep copy of the object.</returns>
        public static T Clone<T>(this T source)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", nameof(source));
            }

            // Don't serialize a null object, simply return the default for that object
            if (ReferenceEquals(source, null)) return default;

            using (Stream stream = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }

        }

        /// <summary>
        /// Perform a deep Copy of the object, using Json as a serialization method. NOTE: Private members are not cloned using this method.
        /// </summary>
        /// <typeparam name="T">The type of object being copied.</typeparam>
        /// <param name="source">The object instance to copy.</param>
        /// <returns>The copied object.</returns>
        public static T CloneJson<T>(this T source)
        {
            // Don't serialize a null object, simply return the default for that object
            if (ReferenceEquals(source, null)) return default;

            // initialize inner objects individually
            // for example in default constructor some list property initialized with some values,
            // but in 'source' these items are cleaned -
            // without ObjectCreationHandling.Replace default constructor values will be added to result
            var deserializeSettings = new JsonSerializerSettings { ObjectCreationHandling = ObjectCreationHandling.Replace };

            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(source), deserializeSettings);
        }
    }

    public static class StaticHelpers
    {
        public static string GetLocation(string ipAddress)
        {
            string url = "http://api.ipstack.com/" + ipAddress + "?access_key=e162ade6d7524e5236473f3e27ef632e";
            var request = WebRequest.Create(url);

            using (WebResponse wrs = request.GetResponse())
            using (Stream stream = wrs.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                string json = reader.ReadToEnd();
                var obj = JObject.Parse(json);
                string City = (string)obj["city"];
                string Country = (string)obj["region_name"];
                string CountryCode = (string)obj["country_code"];

                return (CountryCode + " - " + Country + "," + City);
            }
        }
        private static byte[] GetByteArrayFromImage(IFormFile file)
        {
            using (var target = new MemoryStream())
            {
                file.CopyTo(target);
                return target.ToArray();
            }
        }
        public static string ToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
        //public static string GetQueryString(this object obj)
        //{
        //    var properties = from p in obj.GetType().GetProperties()
        //                     where p.GetValue(obj, null) != null
        //                     select p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(obj, null).ToString());

        //    return String.Join("&", properties.ToArray());
        //}
        public static string GetQueryString<T>(this T dynamicObject)
        {
            var nameValueCollection = new NameValueCollection();
            foreach (PropertyInfo propertyDescriptor in dynamicObject.GetType().GetProperties().Where(x => x.GetValue(dynamicObject, null) != null).ToList())
            {
                string value = propertyDescriptor.GetValue(dynamicObject).ToString();
                nameValueCollection.Add(propertyDescriptor.Name, value);
            }
            return nameValueCollection.ToQueryString();
        }
        public static string ToQueryString(this NameValueCollection nvc)
        {
            if (nvc == null) return string.Empty;

            StringBuilder sb = new StringBuilder();

            foreach (string key in nvc.Keys)
            {
                if (string.IsNullOrWhiteSpace(key)) continue;

                string[] values = nvc.GetValues(key);
                if (values == null) continue;

                foreach (string value in values)
                {
                    sb.Append(sb.Length == 0 ? "?" : "&");
                    sb.AppendFormat("{0}={1}", Uri.EscapeDataString(key), Uri.EscapeDataString(value));
                }
            }

            return sb.ToString();
        }

        public static List<string> GetListOfDescription<T>() where T : struct
        {
            Type t = typeof(T);
            return !t.IsEnum ? null : Enum.GetValues(t).Cast<Enum>().Select(x => x.GetDescription()).ToList();
        }
        public static string GetDescription(this Enum value)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            if (name != null)
            {
                FieldInfo field = type.GetField(name);
                if (field != null)
                {
                    DescriptionAttribute attr = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
                    if (attr != null)
                    {
                        return attr.Description;
                    }
                }
            }
            return null;
            /* how to use
                MyEnum x = MyEnum.NeedMoreCoffee;
                string description = x.GetDescription();
            */

        }

        public static string GetVerificationEmail(string fullName, string url, string unsubscribeUrl)
        {
            return
           $@" <!DOCTYPE html PUBLIC'-//W3C//DTD XHTML 1.0 Transitional//EN''http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'><html xmlns:v='urn:schemas-microsoft-com:vml'><head><meta http-equiv='Content-Type'content='text/html; charset=UTF-8'/><meta name='viewport'content='width=device-width; initial-scale=1.0; maximum-scale=1.0;'/><!--[if!mso]--><!-- --><link href='https://fonts.googleapis.com/css?family=Work+Sans:300,400,500,600,700'rel='stylesheet'><link href='https://fonts.googleapis.com/css?family=Quicksand:300,400,700'rel='stylesheet'><!--<![endif]--><title>Material Design for Bootstrap</title><style type='text/css'>body{{width:100%;background-color:#ffffff;margin:0;padding:0;-webkit-font-smoothing:antialiased;mso-margin-top-alt:0px;mso-margin-bottom-alt:0px;mso-padding-alt:0px 0px 0px 0px;}} p,h1,h2,h3,h4{{margin - top:0;margin-bottom:0;padding-top:0;padding-bottom:0;}} span.preheader{{display:none;font-size:1px;}} html{{width:100%;}}
table{{font - size:14px;border:0;}}@media only screen and(max-width:640px){{.main - header{{font-size:20px!important;}}.main-section-header{{font-size:28px!important;}}.show{{display:block!important;}}.hide{{display:none!important;}}.align-center{{text-align:center!important;}}.no-bg{{background:none!important;}}.main-image img{{width:440px!important;height:auto!important;}}.divider img{{width:440px!important;}}.container590{{width:440px!important;}}.container580{{width:400px!important;}}.main-button{{width:220px!important;}}.section-img img{{width:320px!important;height:auto!important;}}.team-img img{{width:100%!important;height:auto!important;}}@media only screen and(max-width:479px){{.main - header{{font-size:18px!important;}}.main-section-header{{font-size:26px!important;}}.divider img{{width:280px!important;}}.container590{{width:280px!important;}}.container590{{width:280px!important;}}.container580{{width:260px!important;}}.section-img img{{width:280px!important;height:auto!important;}}</style><!--[if gte mso 9]><style type=”text/css”>body{{font - family:arial,sans-serif!important;}}</style><![endif]--></head><body class='respond'leftmargin='0'topmargin='0'marginwidth='0'marginheight='0'><!--pre-header--><table style='display:none!important;'><tr><td><div style='overflow:hidden;display:none;font-size:1px;color:#ffffff;line-height:1px;font-family:Arial;maxheight:0px;max-width:0px;opacity:0;'>Welcome to Eron!</div></td></tr></table><!--pre-header end--><!--header--><table border='0'width='100%'cellpadding='0'cellspacing='0'bgcolor='ffffff'><tr><td align='center'><table border='0'align='center'width='590'cellpadding='0'cellspacing='0'class='container590'><tr><td height='25'style='font-size: 25px; line-height: 25px;'>&nbsp;</td></tr><tr><td align='center'><table border='0'align='center'width='590'cellpadding='0'cellspacing='0'class='container590'><tr><td align='center'height='70'style='height:70px;'><a href=''style='display: block; border-style: none !important; border: 0 !important;'><img width='100'border='0'style='display: block; width: 100px;'src='https://www.eron.gr/images/logo.png'alt=''/></a></td></tr><tr><td align='center'><table width='360 'border='0'cellpadding='0'cellspacing='0'style='border-collapse:collapse; mso-table-lspace:0pt; mso-table-rspace:0pt;'
class='container590 hide'><tr><td width='120'align='center'style='font-size: 14px; font-family: 'Work Sans', Calibri, sans-serif; line-height: 24px;'><a href=''style='color: #312c32; text-decoration: none;'>ADVERTISE</a></td><td width='120'align='center'style='font-size: 14px; font-family: 'Work Sans', Calibri, sans-serif; line-height: 24px;'><a href=''style='color: #312c32; text-decoration: none;'>SALE</a></td><td width='120'align='center'style='font-size: 14px; font-family: 'Work Sans', Calibri, sans-serif; line-height: 24px;'><a href=''style='color: #312c32; text-decoration: none;'>RENT</a></td></tr></table></td></tr></table></td></tr><tr><td height='25'style='font-size: 25px; line-height: 25px;'>&nbsp;</td></tr></table></td></tr></table><!--end header--><!--big image section--><table border='0'width='100%'cellpadding='0'cellspacing='0'bgcolor='ffffff'class='bg_color'><tr><td align='center'><table border='0'align='center'width='590'cellpadding='0'cellspacing='0'class='container590'><tr><td align='center'style='color: #343434; font-size: 24px; font-family: Quicksand, Calibri, sans-serif; font-weight:700;letter-spacing: 3px; line-height: 35px;'
class='main-header'><!--section text======--><div style='line-height: 35px'>Welcome to a new world of <span style='color: #5caad2;'>real estate</span></div></td></tr><tr><td height='10'style='font-size: 10px; line-height: 10px;'>&nbsp;</td></tr><tr><td align='center'><table border='0'width='40'align='center'cellpadding='0'cellspacing='0'bgcolor='eeeeee'><tr><td height='2'style='font-size: 2px; line-height: 2px;'>&nbsp;</td></tr></table></td></tr><tr><td height='20'style='font-size: 20px; line-height: 20px;'>&nbsp;</td></tr><tr><td align='left'><table border='0'width='590'align='center'cellpadding='0'cellspacing='0'class='container590'><tr><td align='left'style='color: #888888; font-size: 16px; font-family: 'Work Sans', Calibri, sans-serif; line-height: 24px;'><!--section text======--><p style='line-height: 24px; margin-bottom:15px;'>Αγαπητέ/ή {fullName},</p><p style='line-height: 24px;margin-bottom:15px;'>Η εγγραφή σας καταχωρήθηκε επιτυχώς.Απομένει να επιβεβαιώσεται το λογαριασμό σας.</p><p style='line-height: 24px; margin-bottom:20px;'>Για την επιβεβαίωση του λογαριασμού σας πατήστε το παρακάτω κουμπί.</p><table border='0'align='center'width='180'cellpadding='0'cellspacing='0'bgcolor='5caad2'style='margin-bottom:20px;'><tr><td height='10'style='font-size: 10px; line-height: 10px;'>&nbsp;</td></tr><tr><td align='center'style='color: #ffffff; font-size: 14px; font-family: 'Work Sans', Calibri, sans-serif; line-height: 22px; letter-spacing: 2px;'><!--main section button--><div style='line-height: 22px;'><a href='{url}'style='color: #ffffff; text-decoration: none;'>Επιβεβαίωση/Verification</a></div></td></tr><tr><td height='10'style='font-size: 10px; line-height: 10px;'>&nbsp;</td></tr></table><p style='line-height: 24px'>Love,</br>The Eron Team</p></td></tr></table></td></tr></table></td></tr><tr><td height='40'style='font-size: 40px; line-height: 40px;'>&nbsp;</td></tr></table><!--end section--><!--contact section--><table border='0'width='100%'cellpadding='0'cellspacing='0'bgcolor='ffffff'class='bg_color'><tr><td height='60'style='font-size: 60px; line-height: 60px;'>&nbsp;</td></tr><tr><td align='center'><table border='0'align='center'width='590'cellpadding='0'cellspacing='0'class='container590 bg_color'><tr><td align='center'><table border='0'align='center'width='590'cellpadding='0'cellspacing='0'class='container590 bg_color'><tr><td><table border='0'width='300'align='left'cellpadding='0'cellspacing='0'style='border-collapse:collapse; mso-table-lspace:0pt; mso-table-rspace:0pt;'
class='container590'><tr><!--logo--><td align='left'><a href=''style='display: block; border-style: none !important; border: 0 !important;'><img width='80'border='0'style='display: block; width: 150px;'src='https://www.eron.gr/images/logo.png'alt=''/></a></td></tr><tr><td height='25'style='font-size: 25px; line-height: 25px;'>&nbsp;</td></tr><tr><td align='left'style='color: #888888; font-size: 14px; font-family: 'Work Sans', Calibri, sans-serif; line-height: 23px;'
class='text_color'><div style='color: #333333; font-size: 14px; font-family: 'Work Sans', Calibri, sans-serif; font-weight: 600; mso-line-height-rule: exactly; line-height: 23px;'>Email us:<br/><a href='mailto:'style='color: #888888; font-size: 14px; font-family: 'Hind Siliguri', Calibri, Sans-serif; font-weight: 400;'>contact@eron.gr</a></div></td></tr></table><table border='0'width='2'align='left'cellpadding='0'cellspacing='0'style='border-collapse:collapse; mso-table-lspace:0pt; mso-table-rspace:0pt;'
class='container590'><tr><td width='2'height='10'style='font-size: 10px; line-height: 10px;'></td></tr></table><table border='0'width='200'align='right'cellpadding='0'cellspacing='0'style='border-collapse:collapse; mso-table-lspace:0pt; mso-table-rspace:0pt;'
class='container590'><tr><td class='hide'height='45'style='font-size: 45px; line-height: 45px;'>&nbsp;</td></tr><tr><td height='15'style='font-size: 15px; line-height: 15px;'>&nbsp;</td></tr><tr><td><table border='0'align='right'cellpadding='0'cellspacing='0'><tr><td><a href='https://www.facebook.com/Eron-Real-Estate-102609551807645'style='display: block; border-style: none !important; border: 0 !important;'><img width='24'border='0'style='display: block;'src='http://i.imgur.com/RBRORq1.png'alt=''></a></td><td>&nbsp;&nbsp;&nbsp;&nbsp;</td><td><!--<a href='https://twitter.com/MDBootstrap'style='display: block; border-style: none !important; border: 0 !important;'><img width='24'border='0'style='display: block;'src='http://i.imgur.com/Qc3zTxn.png'alt=''></a>--></td><td>&nbsp;&nbsp;&nbsp;&nbsp;</td><td><!--<a href='https://plus.google.com/u/0/b/107863090883699620484/107863090883699620484/posts'style='display: block; border-style: none !important; border: 0 !important;'><img width='24'border='0'style='display: block;'src='http://i.imgur.com/Wji3af6.png'alt=''></a>--></td></tr></table></td></tr></table></td></tr></table></td></tr></table></td></tr><tr><td height='60'style='font-size: 60px; line-height: 60px;'>&nbsp;</td></tr></table><!--end section--><!--footer======--><table border='0'width='100%'cellpadding='0'cellspacing='0'bgcolor='f4f4f4'><tr><td height='25'style='font-size: 25px; line-height: 25px;'>&nbsp;</td></tr><tr><td align='center'><table border='0'align='center'width='590'cellpadding='0'cellspacing='0'class='container590'><tr><td><table border='0'align='left'cellpadding='0'cellspacing='0'style='border-collapse:collapse; mso-table-lspace:0pt; mso-table-rspace:0pt;' 
class='container590'><tr><td align='left'style='color: #aaaaaa; font-size: 14px; font-family: 'Work Sans', Calibri, sans-serif; line-height: 24px;'><div style='line-height: 24px;'><span style='color: #333333;'>Advertisment of Real Estate</span></div></td></tr></table><table border='0'align='left'width='5'cellpadding='0'cellspacing='0'style='border-collapse:collapse; mso-table-lspace:0pt; mso-table-rspace:0pt;'
class='container590'><tr><td height='20'width='5'style='font-size: 20px; line-height: 20px;'>&nbsp;</td></tr></table><table border='0'align='right'cellpadding='0'cellspacing='0'style='border-collapse:collapse; mso-table-lspace:0pt; mso-table-rspace:0pt;'
class='container590'><tr><td align='center'><table align='center'border='0'cellpadding='0'cellspacing='0'><tr><td align='center'><a style='font-size: 14px; font-family: 'Work Sans', Calibri, sans-serif; line-height: 24px;color: #5caad2; text-decoration: none;font-weight:bold;'
href='{unsubscribeUrl}'>UNSUBSCRIBE</a></td></tr></table></td></tr></table></td></tr></table></td></tr><tr><td height='25'style='font-size: 25px; line-height: 25px;'>&nbsp;</td></tr></table><!--end footer======--></body></html>";
        }

        public static string GetContactEmail(string name, string message, string phone, string email, string subject)
        {
            return $@"<!DOCTYPE html PUBLIC'-//W3C//DTD XHTML 1.0 Transitional//EN''http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'><html xmlns:v='urn:schemas-microsoft-com:vml'><head><meta http-equiv='Content-Type'content='text/html; charset=UTF-8'/><meta name='viewport'content='width=device-width; initial-scale=1.0; maximum-scale=1.0;'/><!--[if!mso]--><!-- --><link href='https://fonts.googleapis.com/css?family=Work+Sans:300,400,500,600,700'rel='stylesheet'><link href='https://fonts.googleapis.com/css?family=Quicksand:300,400,700'rel='stylesheet'><!--<![endif]--><title>Eron</title><style type='text/css'>body{{width:100%;background-color:#ffffff;margin:0;padding:0;-webkit-font-smoothing:antialiased;mso-margin-top-alt:0px;mso-margin-bottom-alt:0px;mso-padding-alt:0px 0px 0px 0px;}}
p,h1,h2,h3,h4{{margin-top:0;margin-bottom:0;padding-top:0;padding-bottom:0;}}
span.preheader{{display:none;font-size:1px;}}
html{{width:100%;}}
table{{font-size:14px;border:0;}}@media only screen and(max-width:640px){{.main-header{{font-size:20px!important;}}.main-section-header{{font-size:28px!important;}}.show{{display:block!important;}}.hide{{display:none!important;}}.align-center{{text-align:center!important;}}.no-bg{{background:none!important;}}.main-image img{{width:440px!important;height:auto!important;}}.divider img{{width:440px!important;}}.container590{{width:440px!important;}}.container580{{width:400px!important;}}.main-button{{width:220px!important;}}.section-img img{{width:320px!important;height:auto!important;}}.team-img img{{width:100%!important;height:auto!important;}}}}@media only screen and(max-width:479px){{.main-header{{font-size:18px!important;}}.main-section-header{{font-size:26px!important;}}.divider img{{width:280px!important;}}.container590{{width:280px!important;}}.container590{{width:280px!important;}}.container580{{width:260px!important;}}.section-img img{{width:280px!important;height:auto!important;}}}}</style><!--[if gte mso 9]><style type=”text/css”>body{{font-family:arial,sans-serif!important;}}</style><![endif]--></head><body class='respond'leftmargin='0'topmargin='0'marginwidth='0'marginheight='0'><!--pre-header--><table style='display:none!important;'><tr><td><div style='overflow:hidden;display:none;font-size:1px;color:#ffffff;line-height:1px;font-family:Arial;maxheight:0px;max-width:0px;opacity:0;'>Welcome to Eron!</div></td></tr></table><!--pre-header end--><!--header--><table border='0'width='100%'cellpadding='0'cellspacing='0'bgcolor='ffffff'><tr><td align='center'><table border='0'align='center'width='590'cellpadding='0'cellspacing='0'class='container590'><tr><td height='25'style='font-size: 25px; line-height: 25px;'>&nbsp;</td></tr><tr><td align='center'><table border='0'align='center'width='590'cellpadding='0'cellspacing='0'class='container590'><tr><td align='center'height='70'style='height:70px;'><a href=''style='display: block; border-style: none !important; border: 0 !important;'><img width='100'border='0'style='display: block; width: 100px;'src='https://www.eron.gr/images/logo.png'alt=''/></a></td></tr><tr><td align='center'><table width='360 'border='0'cellpadding='0'cellspacing='0'style='border-collapse:collapse; mso-table-lspace:0pt; mso-table-rspace:0pt;'
class='container590 hide'><tr><td width='120'align='center'style='font-size: 14px; font-family: 'Work Sans', Calibri, sans-serif; line-height: 24px;'><a href=''style='color: #312c32; text-decoration: none;'>ADVERTISE</a></td><td width='120'align='center'style='font-size: 14px; font-family: 'Work Sans', Calibri, sans-serif; line-height: 24px;'><a href=''style='color: #312c32; text-decoration: none;'>SALE</a></td><td width='120'align='center'style='font-size: 14px; font-family: 'Work Sans', Calibri, sans-serif; line-height: 24px;'><a href=''style='color: #312c32; text-decoration: none;'>RENT</a></td></tr></table></td></tr></table></td></tr><tr><td height='25'style='font-size: 25px; line-height: 25px;'>&nbsp;</td></tr></table></td></tr></table><!--end header--><!--big image section--><table border='0'width='100%'cellpadding='0'cellspacing='0'bgcolor='ffffff'class='bg_color'><tr><td align='center'><table border='0'align='center'width='590'cellpadding='0'cellspacing='0'class='container590'><tr><td align='center'style='color: #343434; font-size: 24px; font-family: Quicksand, Calibri, sans-serif; font-weight:700;letter-spacing: 3px; line-height: 35px;'
class='main-header'><!--section text======--><div style='line-height: 35px'>Welcome to a new world of <span style='color: #5caad2;'>real estate</span></div></td></tr><tr><td height='10'style='font-size: 10px; line-height: 10px;'>&nbsp;</td></tr><tr><td align='center'><table border='0'width='40'align='center'cellpadding='0'cellspacing='0'bgcolor='eeeeee'><tr><td height='2'style='font-size: 2px; line-height: 2px;'>&nbsp;</td></tr></table></td></tr><tr><td height='20'style='font-size: 20px; line-height: 20px;'>&nbsp;</td></tr><tr><td align='left'><table border='0'width='590'align='center'cellpadding='0'cellspacing='0'class='container590'><tr><td align='left'style='color: #888888; font-size: 16px; font-family: 'Work Sans', Calibri, sans-serif; line-height: 24px;'><!--section text======--><p style='line-height: 24px; margin-bottom:15px;'>Subject:{subject}</p><p style='line-height: 24px; margin-bottom:15px;'>Message from Eron.gr-Contact Form:</p><p style='line-height: 24px;margin-bottom:15px;'> {message} </p><p style='line-height: 24px; margin-bottom:20px;'>Contact Phone: {phone}</p><p style='line-height: 24px; margin-bottom:20px;'>Contact Email: {email}</p><p style='line-height: 24px'>With Regards,</br> </br> {name}</p></td></tr></table></td></tr></table></td></tr><tr><td height='40'style='font-size: 40px; line-height: 40px;'>&nbsp;</td></tr></table><!--end section--><!--contact section--><table border='0'width='100%'cellpadding='0'cellspacing='0'bgcolor='ffffff'class='bg_color'><tr><td height='60'style='font-size: 60px; line-height: 60px;'>&nbsp;</td></tr><tr><td align='center'><table border='0'align='center'width='590'cellpadding='0'cellspacing='0'class='container590 bg_color'><tr><td align='center'><table border='0'align='center'width='590'cellpadding='0'cellspacing='0'class='container590 bg_color'><tr><td><table border='0'width='300'align='left'cellpadding='0'cellspacing='0'style='border-collapse:collapse; mso-table-lspace:0pt; mso-table-rspace:0pt;'
class='container590'><tr><!--logo--><td align='left'><a href=''style='display: block; border-style: none !important; border: 0 !important;'><img width='80'border='0'style='display: block; width: 150px;'src='https://www.eron.gr/images/logo.png'alt=''/></a></td></tr><tr><td height='25'style='font-size: 25px; line-height: 25px;'>&nbsp;</td></tr><tr><td align='left'style='color: #888888; font-size: 14px; font-family: 'Work Sans', Calibri, sans-serif; line-height: 23px;'
class='text_color'><div style='color: #333333; font-size: 14px; font-family: 'Work Sans', Calibri, sans-serif; font-weight: 600; mso-line-height-rule: exactly; line-height: 23px;'>Email us:<br/><a href='mailto:'style='color: #888888; font-size: 14px; font-family: 'Hind Siliguri', Calibri, Sans-serif; font-weight: 400;'>contact@eron.gr</a></div></td></tr></table><table border='0'width='2'align='left'cellpadding='0'cellspacing='0'style='border-collapse:collapse; mso-table-lspace:0pt; mso-table-rspace:0pt;'
class='container590'><tr><td width='2'height='10'style='font-size: 10px; line-height: 10px;'></td></tr></table><table border='0'width='200'align='right'cellpadding='0'cellspacing='0'style='border-collapse:collapse; mso-table-lspace:0pt; mso-table-rspace:0pt;'
class='container590'><tr><td class='hide'height='45'style='font-size: 45px; line-height: 45px;'>&nbsp;</td></tr><tr><td height='15'style='font-size: 15px; line-height: 15px;'>&nbsp;</td></tr><tr><td><table border='0'align='right'cellpadding='0'cellspacing='0'><tr><td><a href='https://www.facebook.com/eron.2021'style='display: block; border-style: none !important; border: 0 !important;'><img width='24'border='0'style='display: block;'src='http://i.imgur.com/RBRORq1.png'alt=''></a></td><td>&nbsp;&nbsp;&nbsp;&nbsp;</td><td><a href='https://twitter.com/eron_gr'style='display: block; border-style: none !important; border: 0 !important;'><img width='24'border='0'style='display: block;'src='http://i.imgur.com/Qc3zTxn.png'alt=''></a></td><td>&nbsp;&nbsp;&nbsp;&nbsp;</td><td><!--<a href='https://plus.google.com/u/0/b/107863090883699620484/107863090883699620484/posts'style='display: block; border-style: none !important; border: 0 !important;'><img width='24'border='0'style='display: block;'src='http://i.imgur.com/Wji3af6.png'alt=''></a>--></td></tr></table></td></tr></table></td></tr></table></td></tr></table></td></tr><tr><td height='60'style='font-size: 60px; line-height: 60px;'>&nbsp;</td></tr></table><!--end section--><!--footer======--><table border='0'width='100%'cellpadding='0'cellspacing='0'bgcolor='f4f4f4'><tr><td height='25'style='font-size: 25px; line-height: 25px;'>&nbsp;</td></tr><tr><td align='center'><table border='0'align='center'width='590'cellpadding='0'cellspacing='0'class='container590'><tr><td><table border='0'align='left'cellpadding='0'cellspacing='0'style='border-collapse:collapse; mso-table-lspace:0pt; mso-table-rspace:0pt;'
class='container590'><tr><td align='left'style='color: #aaaaaa; font-size: 14px; font-family: 'Work Sans', Calibri, sans-serif; line-height: 24px;'><div style='line-height: 24px;'><span style='color: #333333;'>Advertisment of Real Estate</span></div></td></tr></table><table border='0'align='left'width='5'cellpadding='0'cellspacing='0'style='border-collapse:collapse; mso-table-lspace:0pt; mso-table-rspace:0pt;'
class='container590'><tr><td height='20'width='5'style='font-size: 20px; line-height: 20px;'>&nbsp;</td></tr></table><table border='0'align='right'cellpadding='0'cellspacing='0'style='border-collapse:collapse; mso-table-lspace:0pt; mso-table-rspace:0pt;'
class='container590'><tr><td align='center'><table align='center'border='0'cellpadding='0'cellspacing='0'><tr><td align='center'><a style='font-size: 14px; font-family: 'Work Sans', Calibri, sans-serif; line-height: 24px;color: #5caad2; text-decoration: none;font-weight:bold;'>Piraeus,Greece</a></td></tr></table></td></tr></table></td></tr></table></td></tr><tr><td height='25'style='font-size: 25px; line-height: 25px;'>&nbsp;</td></tr></table><!--end footer======--></body></html>";
        }
    }
}
