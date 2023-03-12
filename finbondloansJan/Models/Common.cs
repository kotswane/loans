using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace StaffLoans.Models
{
    public class Common
    {
        public const string SESS_LOGGED_IN_CLIENT_CODE = "LoggedInClientCODE";
        public const string SESS_LOGGED_IN_CLIENT_ID_NUMBER = "LoggedInCLIENTIDNumber";
        public const string SESS_LOGGED_IN_CLIENT_USN = "LoggedInCLIENTUSN";
        public const string SESS_LOGGED_IN_CLIENT_NAME = "LoggedInCLIENTeName";
        public const string SESS_LOGGED_IN_CLIENT_SURNAME = "LoggedInCLIENTeSurName";
        public const string SESS_LOGGED_IN_CLIENT_CONTACTNO = "LoggedInCLIENTeContactNo";
        public const string SESS_LOGGED_IN_CLIENT_ADDRESS = "LoggedInCLIENTAddress";
        public const string SESS_LOGGED_IN_CLIENT_ADDRESS2 = "LoggedInCLIENTAddress2";
        public const string SESS_LOGGED_IN_CLIENT_EMAIL = "LoggedInCLIENTEmail";
        public const string SESS_APPLICATION_NUMBER = "LoanApplicationNumber";
        public const string SESS_L_SSESSION_ID = "SessionID";
        public const string SESS_CLIENTLOGON_SSESSION_ID = "ClientSessionID";
        public const string SESS_LOANS_EXISTS = "LoansExists";


        public static string ClientLoginSessionId
        {
            get
            {
                if (HttpContext.Current.Session[SESS_CLIENTLOGON_SSESSION_ID] != null)
                    return HttpContext.Current.Session[SESS_CLIENTLOGON_SSESSION_ID].ToString();
                else
                    return "";
            }
            set
            {
                HttpContext.Current.Session[SESS_CLIENTLOGON_SSESSION_ID] = value;
            }

        }

        public static bool  ClientLoansExists
        {
            get
            {
                if (HttpContext.Current.Session[SESS_LOANS_EXISTS] != null)
                    return Convert.ToBoolean( HttpContext.Current.Session[SESS_LOANS_EXISTS]);
                else
                    return false ;
            }
            set
            {
                HttpContext.Current.Session[SESS_LOANS_EXISTS] = value;
            }

        }
        public static string OperatorLoginSessionId
        {
            get
            {
                if (HttpContext.Current.Session[SESS_L_SSESSION_ID] != null)
                    return HttpContext.Current.Session[SESS_L_SSESSION_ID].ToString();
                else
                    return "";
            }
            set
            {
                HttpContext.Current.Session[SESS_L_SSESSION_ID] = value;
            }

        }
        private static readonly ILog wLogger = LogManager.GetLogger("InfoLogs");
        public static LoanAPI.TSessionData getOperatorSessionData()
        {
            LoanAPI.TSessionData sess = new LoanAPI.TSessionData();
            if(OperatorLoginSessionId=="" || OperatorLoginSessionId==null )
            {
                using (var proxy = new LoanAPI.FINBONDAPIClient())
                {
                    LoanAPI.TLogonReply loginReply = new LoanAPI.TLogonReply();
                    wLogger.Info("OperatorLogon request");
                    loginReply = proxy.OperatorLogon(ConfigurationManager.AppSettings["OperatorLoginIDnumber"], "", "", 1, Convert.ToUInt32(ConfigurationManager.AppSettings["Origin"].ToString()));
                    wLogger.Info("OperatorLogon response" + JsonConvert.SerializeObject(loginReply));
                    OperatorLoginSessionId = loginReply.SessionData.SessionID;
                }
            }
            sess.SessionID = OperatorLoginSessionId;
            return sess;

        }

        public static LoanAPI.TSessionData getClientSessionData()
        {
            LoanAPI.TSessionData sess = new LoanAPI.TSessionData();
            
            sess.SessionID = ClientLoginSessionId;
            return sess;

        }
        public static int LoggedInClientCode
        {
            get
            {
                if (HttpContext.Current.Session[SESS_LOGGED_IN_CLIENT_CODE] != null)
                    return (int)HttpContext.Current.Session[SESS_LOGGED_IN_CLIENT_CODE];
                else
                    return 0;
            }
            set
            {
                HttpContext.Current.Session[SESS_LOGGED_IN_CLIENT_CODE] = value;
            }
        }

        public static string  LoggedInClientName
        {
            get
            {
                if (HttpContext.Current.Session[SESS_LOGGED_IN_CLIENT_NAME] != null)
                    return (string)HttpContext.Current.Session[SESS_LOGGED_IN_CLIENT_NAME];
                else
                    return "";
            }
            set
            {
                HttpContext.Current.Session[SESS_LOGGED_IN_CLIENT_NAME] = value;
            }
        }

        public static string LoggedInClientSurName
        {
            get
            {
                if (HttpContext.Current.Session[SESS_LOGGED_IN_CLIENT_SURNAME] != null)
                    return (string)HttpContext.Current.Session[SESS_LOGGED_IN_CLIENT_SURNAME];
                else
                    return "";
            }
            set
            {
                HttpContext.Current.Session[SESS_LOGGED_IN_CLIENT_SURNAME] = value;
            }
        }

        public static string LoggedInClientEmailAddress
        {
            get
            {
                if (HttpContext.Current.Session[SESS_LOGGED_IN_CLIENT_EMAIL] != null)
                    return (string)HttpContext.Current.Session[SESS_LOGGED_IN_CLIENT_EMAIL];
                else
                    return "";
            }
            set
            {
                HttpContext.Current.Session[SESS_LOGGED_IN_CLIENT_EMAIL] = value;
            }
        }

        public static string LoggedInClientContactNo
        {
            get
            {
                if (HttpContext.Current.Session[SESS_LOGGED_IN_CLIENT_CONTACTNO] != null)
                    return (string)HttpContext.Current.Session[SESS_LOGGED_IN_CLIENT_CONTACTNO];
                else
                    return "";
            }
            set
            {
                HttpContext.Current.Session[SESS_LOGGED_IN_CLIENT_CONTACTNO] = value;
            }
        }
        public static string LoggedInClientAddress
        {
            get
            {
                if (HttpContext.Current.Session[SESS_LOGGED_IN_CLIENT_ADDRESS] != null)
                    return (string)HttpContext.Current.Session[SESS_LOGGED_IN_CLIENT_ADDRESS];
                else
                    return "";
            }
            set
            {
                HttpContext.Current.Session[SESS_LOGGED_IN_CLIENT_ADDRESS] = value;
            }
        }
        public static string LoggedInClientAddress2
        {
            get
            {
                if (HttpContext.Current.Session[SESS_LOGGED_IN_CLIENT_ADDRESS2] != null)
                    return (string)HttpContext.Current.Session[SESS_LOGGED_IN_CLIENT_ADDRESS2];
                else
                    return "";
            }
            set
            {
                HttpContext.Current.Session[SESS_LOGGED_IN_CLIENT_ADDRESS2] = value;
            }
        }

        public static string LoanApplicationNumber
        {
            get
            {
                if (HttpContext.Current.Session[SESS_APPLICATION_NUMBER] != null)
                    return (string)HttpContext.Current.Session[SESS_APPLICATION_NUMBER];
                else
                    return "";
            }
            set
            {
                HttpContext.Current.Session[SESS_APPLICATION_NUMBER] = value;
            }
        }


        public static string LoggedInClientIDNumber
        {
            get
            {
                if (HttpContext.Current.Session[SESS_LOGGED_IN_CLIENT_ID_NUMBER] != null)
                    return (string)HttpContext.Current.Session[SESS_LOGGED_IN_CLIENT_ID_NUMBER];
                else
                    return "";
            }
            set
            {
                HttpContext.Current.Session[SESS_LOGGED_IN_CLIENT_ID_NUMBER] = value;
            }
        }

        public static string LoggedInClientUSN
        {
            get
            {
                if (HttpContext.Current.Session[SESS_LOGGED_IN_CLIENT_USN] != null)
                    return (string)HttpContext.Current.Session[SESS_LOGGED_IN_CLIENT_USN];
                else
                    return "";
            }
            set
            {
                HttpContext.Current.Session[SESS_LOGGED_IN_CLIENT_USN] = value;
            }
        }

        //public static string UploadedIDDocumentName
        //{
        //    get
        //    {
        //        if (HttpContext.Current.Session["UploadedIDDocumentName"] == null)
        //            return null;
        //        return (string)HttpContext.Current.Session["UploadedIDDocumentName"];
        //    }
        //    set
        //    {
        //        HttpContext.Current.Session["UploadedIDDocumentName"] = value;
        //    }
        //}

        //public static string UploadedIDFileExtension
        //{
        //    get
        //    {
        //        if (HttpContext.Current.Session["UploadedIDFileExtension"] == null)
        //            return null;
        //        return (string)HttpContext.Current.Session["UploadedIDFileExtension"];
        //    }
        //    set
        //    {
        //        HttpContext.Current.Session["UploadedIDFileExtension"] = value;
        //    }
        //}

        //public static byte[] UploadedIDDocument
        //{
        //    get
        //    {
        //        if (HttpContext.Current.Session["UploadedIDDocument"] == null)
        //            return null;
        //        return (byte[])HttpContext.Current.Session["UploadedIDDocument"];
        //    }
        //    set
        //    {
        //        HttpContext.Current.Session["UploadedIDDocument"] = value;
        //    }
        //}

        //public static string UploadedPayslipDocumentName
        //{
        //    get
        //    {
        //        if (HttpContext.Current.Session["UploadedPayslipDocumentName"] == null)
        //            return null;
        //        return (string)HttpContext.Current.Session["UploadedPayslipDocumentName"];
        //    }
        //    set
        //    {
        //        HttpContext.Current.Session["UploadedPayslipDocumentName"] = value;
        //    }
        //}

        //public static string UploadedPaySlipFileExtension
        //{
        //    get
        //    {
        //        if (HttpContext.Current.Session["UploadedPaySlipFileExtension"] == null)
        //            return null;
        //        return (string)HttpContext.Current.Session["UploadedPaySlipFileExtension"];
        //    }
        //    set
        //    {
        //        HttpContext.Current.Session["UploadedPaySlipFileExtension"] = value;
        //    }
        //}

        //public static byte[] UploadedPayslipDocument
        //{
        //    get
        //    {
        //        if (HttpContext.Current.Session["UploadedPayslipDocument"] == null)
        //            return null;
        //        return (byte[])HttpContext.Current.Session["UploadedPayslipDocument"];
        //    }
        //    set
        //    {
        //        HttpContext.Current.Session["UploadedPayslipDocument"] = value;
        //    }
        //}


        //public static string UploadedBankStatementDocumentName
        //{
        //    get
        //    {
        //        if (HttpContext.Current.Session["UploadedBankStatementDocumentName"] == null)
        //            return null;
        //        return (string)HttpContext.Current.Session["UploadedBankStatementDocumentName"];
        //    }
        //    set
        //    {
        //        HttpContext.Current.Session["UploadedBankStatementDocumentName"] = value;
        //    }
        //}
        //public static string UploadedBankStatementExtension
        //{
        //    get
        //    {
        //        if (HttpContext.Current.Session["UploadedBankStatementExtension"] == null)
        //            return null;
        //        return (string)HttpContext.Current.Session["UploadedBankStatementExtension"];
        //    }
        //    set
        //    {
        //        HttpContext.Current.Session["UploadedBankStatementExtension"] = value;
        //    }
        //}

        //public static byte[] UploadedBankStatementDocument
        //{
        //    get
        //    {
        //        if (HttpContext.Current.Session["UploadedBankStatementDocument"] == null)
        //            return null;
        //        return (byte[])HttpContext.Current.Session["UploadedBankStatementDocument"];
        //    }
        //    set
        //    {
        //        HttpContext.Current.Session["UploadedBankStatementDocument"] = value;
        //    }
        //}

        public static string UploadedSelfieDocumentName
        {
            get
            {
                if (HttpContext.Current.Session["UploadedSelfieDocumentName"] == null)
                    return null;
                return (string)HttpContext.Current.Session["UploadedSelfieDocumentName"];
            }
            set
            {
                HttpContext.Current.Session["UploadedSelfieDocumentName"] = value;
            }
        }
        public static string UploadedSelfieExtension
        {
            get
            {
                if (HttpContext.Current.Session["UploadedSelfieExtension"] == null)
                    return null;
                return (string)HttpContext.Current.Session["UploadedSelfieExtension"];
            }
            set
            {
                HttpContext.Current.Session["UploadedSelfieExtension"] = value;
            }
        }

        public static byte[] UploadedSelfieDocument
        {
            get
            {
                if (HttpContext.Current.Session["UploadedSelfieDocument"] == null)
                    return null;
                return (byte[])HttpContext.Current.Session["UploadedSelfieDocument"];
            }
            set
            {
                HttpContext.Current.Session["UploadedSelfieDocument"] = value;
            }
        }

        public static List<Document> UploadedDocumentList
        {
            get
            {
                if (HttpContext.Current.Session["UploadedDocumentList"] == null)
                    return null;
                return (List<Document>)HttpContext.Current.Session["UploadedDocumentList"];
            }
            set
            {
                HttpContext.Current.Session["UploadedDocumentList"] = value;
            }

        }

        public static string GetLoggedInDate()
        {
            return DateTime.Now.ToShortDateString();
        }

        public static string GetLoggedInBranch()
        {
            return "RoseBank";
        }
        public static string GetExtension(string strFilename)
        {
            if (strFilename.ToString().IndexOf("jpeg")> 0)
                return strFilename.ToString().Substring(strFilename.ToString().LastIndexOf(".") + 1, 4);
            else
                return strFilename.ToString().Substring(strFilename.ToString().LastIndexOf(".") + 1, 3);
        }
        public static bool CheckValidFileFormat(string strExtension)
        {
            if (strExtension == "PDF" || strExtension == "PNG" || strExtension == "JPG" || strExtension == "JPEG"
                          || strExtension == "GIF" || strExtension == "TIF"
                          || strExtension == "BMP")
            {
                return true;
            }
            else
                return false;
        }
        public static bool CheckImageFileFormat(string strExtension)
        {
            if (strExtension == "PNG" || strExtension == "JPG" || strExtension == "JPEG"
                          || strExtension == "GIF" || strExtension == "TIF"
                          || strExtension == "BMP")
            {
                return true;
            }
            else
                return false;
        }

        #region Supporting documents Validate

        [DllImport(@"urlmon.dll", CharSet = CharSet.Auto)]
        private extern static UInt32 FindMimeFromData(UInt32 pBC,
        [MarshalAs(UnmanagedType.LPStr)] String pwzUrl,
        [MarshalAs(UnmanagedType.LPArray)] byte[] pBuffer,
        UInt32 cbSize, [MarshalAs(UnmanagedType.LPStr)] String pwzMimeProposed,
        UInt32 dwMimeFlags,
        out UInt32 ppwzMimeOut,
        UInt32 dwReserverd);

        public static bool ValidateUploadedgDoc(string strFileName, byte[] document, string strContentType)
        {
            if (string.IsNullOrEmpty(strFileName))
            {
                return false;
            }

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // Mime Type Check
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////


            //UInt32 mimetype;
            //FindMimeFromData(0, null, document, 256, null, 0, out mimetype, 0);
            //IntPtr mimeTypePtr = new IntPtr(mimetype);
            //string mime = Marshal.PtrToStringUni(mimeTypePtr);
            //Marshal.FreeCoTaskMem(mimeTypePtr);
            string extension = strFileName.Substring(strFileName.LastIndexOf(".") + 1);

            ////https://www.sitepoint.com/web-foundations/mime-types-complete-list/           
            string mime = strContentType;
            bool isFirstCheckValid = false;
            switch (extension.ToUpper())
            {
                case "PDF":
                    if (mime == "application/pdf")
                        isFirstCheckValid = true;
                    break;
                case "PNG":
                    if (mime == "image/x-png" || mime == "image/png")
                        isFirstCheckValid = true;
                    break;
                case "JPG":
                case "JPEG":
                    if (mime == "image/jpeg" || mime == "image/pjpeg")
                        isFirstCheckValid = true;
                    break;
                case "GIF":
                    if (mime == "image/gif")
                        isFirstCheckValid = true;
                    break;
                case "TIF":
                case "TIFF":
                    if (mime == "image/tiff" || mime == "image/x-tiff")
                        isFirstCheckValid = true;
                    break;
                case "BMP":
                    if (mime == "image/bmp" || mime == "image/x-windows-bmp")
                        isFirstCheckValid = true;
                    break;

            }


            /////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // Magic Number Check
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////

            string hexadecimalString = BitConverter.ToString(document);
            hexadecimalString.Replace("-", "");

            //https://en.wikipedia.org/wiki/Magic_number_(programming)
            //GIF image files have the ASCII code for "GIF89a" (47 49 46 38 39 61) or "GIF87a" (47 49 46 38 37 61)
            //JPEG image files begin with FF D8 and end with FF D9. JPEG/JFIF files contain the ASCII code for "JFIF" (4A 46 49 46) as a null terminated string. JPEG/Exif files contain the ASCII code for "Exif" (45 78 69 66) also as a null terminated string, followed by more metadata about the file.
            //PNG image files begin with an 8-byte signature which identifies the file as a PNG file and allows detection of common file transfer problems: \211 P N G \r \n \032 \n (89 50 4E 47 0D 0A 1A 0A). That signature contains various newline characters to permit detecting unwarranted automated newline conversions, such as transferring the file using FTP with the ASCII transfer mode instead of the binary mode.[5]
            //PDF files start with "%PDF" (hex 25 50 44 46).
            //TIFF files begin with either II or MM followed by 42 as a two-byte integer in little or big endian byte ordering. II is for Intel, which uses little endian byte ordering, so the magic number is 49 49 2A 00. MM is for Motorola, which uses big endian byte ordering, so the magic number is 4D 4D 00 2A.

            //string[] hexadecimalStringArray = hexadecimalString.Split(new char[]{'-'});

            bool isSecondCheckValid = false;
            switch (extension.ToUpper())
            {
                case "PDF":
                case "PNG":
                case "JPG":
                case "JPEG":
                case "GIF":
                case "TIF":
                case "TIFF":
                case "BMP":
                    if (
                        (hexadecimalString.StartsWith("25-50-44-46"))
                        || (hexadecimalString.StartsWith("89-50-4E-47-0D-0A-1A-0A"))
                        || (hexadecimalString.StartsWith("FF-D8") && hexadecimalString.EndsWith("FF-D9"))
                        || (hexadecimalString.Contains("47-49-46-38-39-61") || hexadecimalString.Contains("47-49-46-38-37-61"))
                        || (hexadecimalString.StartsWith("49-49-2A-00") || hexadecimalString.StartsWith("4D-4D-00-2A"))
                        || (hexadecimalString.StartsWith("42-4D"))
                        )
                        isSecondCheckValid = true;
                    break;

                    //case "PDF":
                    //    if (hexadecimalString.StartsWith("25-50-44-46"))
                    //        isSecondCheckValid = true;
                    //    break;
                    //case "PNG":
                    //    if (hexadecimalString.StartsWith("89-50-4E-47-0D-0A-1A-0A"))
                    //        isSecondCheckValid = true;
                    //    break;
                    //case "JPG":
                    //case "JPEG":
                    //    if (hexadecimalString.StartsWith("FF-D8") &&
                    //        hexadecimalString.EndsWith("FF-D9"))
                    //        isSecondCheckValid = true;
                    //    break;
                    //case "GIF":
                    //    if (hexadecimalString.Contains("47-49-46-38-39-61") || hexadecimalString.Contains("47-49-46-38-37-61"))
                    //        isSecondCheckValid = true;
                    //    break;
                    //case "TIF":
                    //case "TIFF":
                    //    if (hexadecimalString.StartsWith("49-49-2A-00") || hexadecimalString.StartsWith("4D-4D-00-2A"))
                    //        isSecondCheckValid = true;
                    //    break;
                    //case "BMP":
                    //    if (hexadecimalString.StartsWith("42-4D"))
                    //        isSecondCheckValid = true;
                    //    break;
            }

            if (isFirstCheckValid && isSecondCheckValid)
            //if (isSecondCheckValid)
            {
                return true;
            }
            else
            {
                return false;
            }


        }
        #endregion

        public enum DocumentType
        {
            IDDocument=1,
            Payslip=2,
            BankStatement=3,
            Selfie=8
        }

        public static string FormatMoney(decimal dValue)
        {
            return String.Format(CultureInfo.CreateSpecificCulture("en-ZA"), "{0:C}", dValue).Replace(",", ".");
        }
        public static string FormatMoney(string dValue)
        {
            return String.Format(CultureInfo.CreateSpecificCulture("en-ZA"), "{0:C}", dValue).Replace(",", ".");
            //return "R " + dValue;
        }

        public static string FormatDate(string strDate)
        {
            if (strDate != "0" && strDate !="" && strDate !=null)
            {
                var result = DateTime.ParseExact(strDate,
                       "yyyyMd",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.AssumeLocal);
                //return Convert.ToDateTime(strDate).ToString("yyyy/MM/dd");

                return result.ToString("yyyy/MM/dd");
                // return "R " + dValue;
            }
            else
                return "";
        }

        public static string Encrypt(string ToEncrypt, bool useHasing)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(ToEncrypt);
            //System.Configuration.AppSettingsReader settingsReader = new AppSettingsReader();  
            string Key = "Bhagwati";
            if (useHasing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(Key));
                hashmd5.Clear();
            }
            else
            {
                keyArray = UTF8Encoding.UTF8.GetBytes(Key);
            }
            TripleDESCryptoServiceProvider tDes = new TripleDESCryptoServiceProvider();
            tDes.Key = keyArray;
            tDes.Mode = CipherMode.ECB;
            tDes.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tDes.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            tDes.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public static string Decrypt(string cypherString, bool useHasing)
        {
            byte[] keyArray;
            byte[] toDecryptArray = Convert.FromBase64String(cypherString);
            //byte[] toEncryptArray = Convert.FromBase64String(cypherString); 
            //System.Configuration.AppSettingsReader settingReader = new     AppSettingsReader();
            string key = "Bhagwati";
            if (useHasing)
            {
                MD5CryptoServiceProvider hashmd = new MD5CryptoServiceProvider();
                keyArray = hashmd.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                hashmd.Clear();
            }
            else
            {
                keyArray = UTF8Encoding.UTF8.GetBytes(key);
            }
            TripleDESCryptoServiceProvider tDes = new TripleDESCryptoServiceProvider();
            tDes.Key = keyArray;
            tDes.Mode = CipherMode.ECB;
            tDes.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tDes.CreateDecryptor();
            try
            {
                byte[] resultArray = cTransform.TransformFinalBlock(toDecryptArray, 0,
                    toDecryptArray.Length);
                tDes.Clear();
                return UTF8Encoding.UTF8.GetString(resultArray, 0, resultArray.Length);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static int GenerateOTP()
        {
            int _min = 1000;
            int _max = 9999;
            Random _rdm = new Random();
            return _rdm.Next(_min, _max);
        }

        //public static string getOperatorSessionData()
        //{
        //    return "";
        //}
    }
}