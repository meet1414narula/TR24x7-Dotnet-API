namespace Common.Constants
{
    public class GenericConstant
    {
        public const string MEDIA_TYPE_XML = "application/xml";
        public const string API_RESOURCES = "API.Common.Resources";
        public const string OTP_GENERATE_URL = "API/V1/{0}/SMS/{1}/";
        public const string MESSAGE_SERVICE_URL = "http://2factor.in/API/V1/{0}/ADDON_SERVICES/SEND/TSMS";
        public const string AUTOGEN = "AUTOGEN";
        public const string OTP_VERIFY_URL = "API/V1/{0}/SMS/{1}/{2}";
        public const string SUCCESS = "Success";
        public const string IMG_EXT = ".jpg";
        public const string ERROR = "Error";
        public const string GOODS_ID = "goodsId";
        public const string IMAGE_PATH = "~/Images/";
        public const string IMAGES_BASE_ADDRESS = "http://mjtransport.in/Images/";
        public const int GoodsNoOfDays = 7;
        public const int TokenExpiryDays = 30;
        public const int VehicleNoOfDays = 7;
        public const string FEET = " feet";
        public const string NEW_LINE = @"\n";
        public const string BLANK_SPACE = " ";
        public const string STATUS = "Status";
        public const string DETAILS = "Details";
        public const string FROM = "From";
        public const string TO = "To";
        public const string TEMPLATE_NAME = "TemplateName";
        public const string TEMPLATE_REGISTRATION = "OneTimePassword";
        public const string TEMPLATE_BIDCHANGED = "BIDCH";
        public const string TEMPLATE_BIDACCEPTED = "BIDACPTED";
        public const string TEMPLATE_BIDCONVERTED = "BIDCONVTD";
        public const string TEMPLATE_ENQUIRY = "ENQUIRY";
        public const string TEMPLATE_FORGOTPASSWORD = "FGP";
        public const string MESSAGE_VARIABLE = "VAR";
        public const string BID_CHANGED_MSG = "{0} bid is posted for {1} to {2},Vehicle: {3},{4}";
        public const string BID_ACCEPTED_MSG = "{0} bid accepted for {1} to {2},Vehicle: {3},{4}";
        public const string REGISTRATION_MSG = "{0} is your one time password for phone verification.";
        public const string STATE_CODE = "({0})";

        public const string MOCK_OTP_SERVICE = "MockOTPService";
        public const string OTP_KEY = "OTPAPIKey";
        public const string MESSAGE_SERVICE_SENDER_ID = "MessageServiceSenderId";
        public const string ADMIN_MOBILE_NUMBER = "AdminMobileNumber";
        public const string SUPPORT_NUMBER = "SupportNumber";
        public const string INDIAN_STANDARD_TIME = "India Standard Time";


        public class HEADERS
        {
            public const string USER_ID = "UserId";
            public const string TOKEN = "Token";
        }

        public class METHOD
        {
            public const string GET = "GET";
            public const string POST = "POST";
        }

        public class APPKEYS
        {
            public const string SUPPORT_NUMBER = "SupportNumber";
        }

        public class CONFIGKEYS
        {
            public const string MAX_WEIGHT = "MaxWeight";
            public const string MAX_LENGTH = "MaxLength";
            public const string MIN_WEIGHT = "MinWeight";
            public const string MIN_LENGTH = "MinLength";
        }
    }
}
