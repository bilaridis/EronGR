﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace EronNew.Resources
{
    public class SharedResources
    {

    }

    public class GlobalCultureService
    {
        private readonly IStringLocalizer _localizer;

        private readonly IHtmlLocalizer _htmlLocalizer;

        private readonly IOptions<RequestLocalizationOptions> _locOptions;

        public bool Premium { get; set; }

        public GlobalCultureService(IStringLocalizerFactory factory, IHtmlLocalizerFactory htmlFactory, IOptions<RequestLocalizationOptions> locOptions)
        {
            var type = typeof(SharedResources);
            var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);
            _localizer = factory.Create("SharedResources", assemblyName.Name);
            _htmlLocalizer = htmlFactory.Create("SharedResources", assemblyName.Name);
            _locOptions = locOptions;
        }

        public LocalizedString GetLocalized(string key)
        {
            return _localizer[key];
        }

        public LocalizedHtmlString GetLocalizedHtml(string key)
        {
            return _htmlLocalizer[key];
        }
    }

    public enum LocalizedPhrases
    {

        [Description("Account Management")] AccountManagement,
        [Description("Active")] Active,
        [Description("Ad Code")] AdCode,
        [Description("Ad Management")] AdManagement,
        [Description("Ad Type")] AdType,
        [Description("Additional characteristics")] Additionalcharacteristics,
        [Description("Additional information")] Additionalinformation,
        [Description("Additional Property Features")] AdditionalPropertyFeatures,
        [Description("Address")] Address,
        [Description("Air Condition")] AirCondition,
        [Description("and")] and,
        [Description("Apartment")] Apartment,
        [Description("Apartment Complex")] ApartmentComplex,
        [Description("Area")] Area,
        [Description("Bathrooms")] Bathrooms,
        [Description("BBQ")] BBQ,
        [Description("Bedrooms")] Bedrooms,
        [Description("Building")] Building,
        [Description("Bungalow")] Bungalow,
        [Description("Business building")] Businessbuilding,
        [Description("By clicking <em> Register </em> you automatically agree to our")] ByclickingRegisteryouautomaticallyagreetoour,
        [Description("Cancel")] Cancel,
        [Description("Category")] Category,
        [Description("Change email")] Changeemail,
        [Description("Choose")] Choose,
        [Description("Choose a profile image")] Chooseaprofileimage,
        [Description("Choose your preferred language")] Chooseyourpreferredlanguage,
        [Description("Click here to Logout")] ClickheretoLogout,
        [Description("CloakRoom")] CloakRoom,
        [Description("Close")] Close,
        [Description("Commands")] Commands,
        [Description("Commercial")] Commercial,
        [Description("Company")] Company,
        [Description("Complete")] Complete,
        [Description("Complete Registration")] CompleteRegistration,
        [Description("Completed")] Completed,
        [Description("Conference Room")] ConferenceRoom,
        [Description("Confirm Password")] ConfirmPassword,
        [Description("Construction")] Construction,
        [Description("Construction Year")] ConstructionYear,
        [Description("Contact Info")] ContactInfo,
        [Description("Contact Phone")] ContactPhone,
        [Description("Contact us")] Contactus,
        [Description("Cottage")] Cottage,
        [Description("County/Region")] CountyRegion,
        [Description("Craft Property")] CraftProperty,
        [Description("Create Draft Ad")] CreateDraftAd,
        [Description("Delete")] Delete,
        [Description("Deleted")] Deleted,
        [Description("Demand")] Demand,
        [Description("Desired Information from Buyers")] DesiredInformationfromBuyers,
        [Description("Detached House")] DetachedHouse,
        [Description("Do you need help")] Doyouneedhelp,
        [Description("Draft")] Draft,
        [Description("Drag the photos here")] Dragthephotoshere,
        [Description("Edit")] Edit,
        [Description("Elevator")] Elevator,
        [Description("Email")] Email,
        [Description("euroSymbol")] euroSymbol,
        [Description("Exhibition Area")] ExhibitionArea,
        [Description("Farm/Ranch")] FarmRanch,
        [Description("Fill in the email to have the password reset email sent to you.")] Fillintheemailtohavethepasswordresetemailsenttoyou,
        [Description("Fireplace")] Fireplace,
        [Description("floor")] floor,
        [Description("Forgot your password")] Forgotyourpassword,
        [Description("from")] from,
        [Description("Furniture")] Furniture,
        [Description("Garden")] Garden,
        [Description("Gym")] Gym,
        [Description("Hall")] Hall,
        [Description("Heating System")] HeatingSystem,
        [Description("Hide")] Hide,
        [Description("Highest -> Lowest Price")] HighestLowestPrice,
        [Description("Home Energy Rate")] HomeEnergyRate,
        [Description("Hotel")] Hotel,
        [Description("Houseboat")] Houseboat,
        [Description("Housekeeping")] Housekeeping,
        [Description("How can we help you")] Howcanwehelpyou,
        [Description("How Many Bathrooms Does It Have?")] HowManyBathroomsDoesItHave,
        [Description("How Many Bedrooms Does It Have?")] HowManyBedroomsDoesItHave,
        [Description("Id")] Id,
        [Description("Important Information")] ImportantInformation,
        [Description("Inactive")] Inactive,
        [Description("Industrial Property")] IndustrialProperty,
        [Description("Info Card")] InfoCard,
        [Description("Internet Fiber")] InternetFiber,
        [Description("Join to our Page")] JointoourPage,
        [Description("Living room")] Livingroom,
        [Description("Locked")] Locked,
        [Description("Loft")] Loft,
        [Description("Login")] Login,
        [Description("Login/Register")] LoginRegister,
        [Description("Logout")] Logout,
        [Description("Lowest To Highest")] LowestToHighest,
        [Description("Maisonette")] Maisonette,
        [Description("Mandatory Information")] MandatoryInformation,
        [Description("Master")] Master,
        [Description("Mobile")] Mobile,
        [Description("Mobile Home")] MobileHome,
        [Description("Most Popular")] MostPopular,
        [Description("Municipality / District")] MunicipalityDistrict,
        [Description("My Card")] MyCard,
        [Description("My Favorites")] MyFavorites,
        [Description("My information")] Myinformation,
        [Description("My Notes")] MyNotes,
        [Description("Name")] Name,
        [Description("No")] No,
        [Description("No ads were found according to Search filters.")] NoadswerefoundaccordingtoSearchfilters,
        [Description("number")] number,
        [Description("Obligatory")] Obligatory,
        [Description("Office")] Office,
        [Description("Ok")] Ok,
        [Description("optional")] optional,
        [Description("Other Properties")] OtherProperties,
        [Description("Other Categories")] OtherCategories,
        [Description("Parcels")] Parcels,
        [Description("Parking")] Parking,
        [Description("Parking Area")] ParkingArea,
        [Description("Password")] Password,
        [Description("Penthouse")] Penthouse,
        [Description("Personal settings")] Personalsettings,
        [Description("Pets")] Pets,
        [Description("Photos")] Photos,
        [Description("Place your ad")] Placeyourad,
        [Description("Please enter an email address for this site below and click the Register button to finish logging in.")] PleaseenteranemailaddressforthissitebelowandclicktheRegisterbuttontofinishloggingin,
        [Description("Plot of Land")] PlotofLand,
        [Description("Pool")] Pool,
        [Description("Post a request")] Postarequest,
        [Description("PostalCode")] PostalCode,
        [Description("Prefabricated Home")] PrefabricatedHome,
        [Description("Preview")] Preview,
        [Description("Preview 360")] Preview360,
        [Description("Price")] Price,
        [Description("Print")] Print,
        [Description("Property Area")] PropertyArea,
        [Description("Property Category")] PropertyCategory,
        [Description("Property Features")] PropertyFeatures,
        [Description("Property Price")] PropertyPrice,
        [Description("Property Type")] PropertyType,
        [Description("Public")] Public,
        [Description("Publish")] Publish,
        [Description("Publish Ad")] PublishAd,
        [Description("Publish/Save")] PublishSave,
        [Description("Range Price")] RangePrice,
        [Description("Rating")] Rating,
        [Description("Register")] Register,
        [Description("Register from the applications")] Registerfromtheapplications,
        [Description("Rent")] Rent,
        [Description("Resend")] Resend,
        [Description("Residential")] Residential,
        [Description("Retry")] Retry,
        [Description("Safety door")] Safetydoor,
        [Description("Safety Frames")] SafetyFrames,
        [Description("Sale")] Sale,
        [Description("Save")] Save,
        [Description("Search")] Search,
        [Description("Search Filters")] SearchFilters,
        [Description("Security Alarm")] SecurityAlarm,
        [Description("See Other Seller Ads")] SeeOtherSellerAds,
        [Description("Select the energy rate of the Property (if available)")] SelecttheenergyrateofthePropertyifavailable,
        [Description("Semi-outdoors")] Semioutdoors,
        [Description("Send verification email")] Sendverificationemail,
        [Description("Send your comments <strong>here</strong>.")] Sendyourcommentshere,
        [Description("Send your message")] Sendyourmessage,
        [Description("Shall we check ?")] Shallwecheck,
        [Description("Share")] Share,
        [Description("Shop")] Shop,
        [Description("Show")] Show,
        [Description("Square")] Square,
        [Description("Status Ad")] StatusAd,
        [Description("Storage")] Storage,
        [Description("Studio")] Studio,
        [Description("Subcategory")] Subcategory,
        [Description("Subject")] Subject,
        [Description("Subscribe to our newsletter")] Subscribetoournewsletter,
        [Description("Subtitle")] Subtitle,
        [Description("Surname")] Surname,
        [Description("Terms of use")] Termsofuse,
        [Description("Title/Name")] TitleName,
        [Description("to")] to,
        [Description("Type")] Type,
        [Description("Type your message here")] Typeyourmessagehere,
        [Description("Unlocked")] Unlocked,
        [Description("Uploading photos started")] Uploadingphotosstarted,
        [Description("View")] View,
        [Description("views")] views,
        [Description("Wallet")] Wallet,
        [Description("WC")] WC,
        [Description("WebSite")] WebSite,
        [Description("WishList")] WishList,
        [Description("Write a short description")] Writeashortdescription,
        [Description("Write To Us")] WriteToUs,
        [Description("Year of Renovation")] YearofRenovation,
        [Description("Yes")] Yes,
        [Description("You can also connect from the applications")] Youcanalsoconnectfromtheapplications,
        [Description("You can choose what amenities are available for the property")] Youcanchoosewhatamenitiesareavailablefortheproperty,
        [Description("You can reset your password now.")] Youcanresetyourpasswordnow,
        [Description("You do not have access to this resource.")] Youdonothaveaccesstothisresource,
        [Description("You have successfully logged out of the application.")] Youhavesuccessfullyloggedoutoftheapplication,
        [Description("You've successfully authenticated with")] Youvesuccessfullyauthenticatedwith,
        [Description("Submit")] Submit,
        [Description("Notes")] Notes,
        [Description("Submit Post")] SubmitPost,
        [Description("greater than 100")] moreThan100,
        [Description("only Number")] onlyInteger,
        [Description("minimum Three Characters")] minimumThreeCharacters,
        [Description("valid City")] validCity,
        [Description("maximum of Characters are 5")] maximumCharacters5,
        [Description("Ask for the Price")] AskforthePrice,
        [Description("My Orders")] MyOrders,
        [Description("Premium")] Premium,
        [Description("Premium Features")] PremiumFeatures,
        [Description("Recent Posts")] RecentPosts,
        [Description("Control Panel")] ControlPanel,
        [Description("Recent Searches")] RecentSearches,
        [Description("Saved Searches")] SavedSearches,
        [Description("Hidden Searches")] HiddenSearches,
        [Description("<strong>Boosted</strong> Advertisement")] HtmlBoostedAdvertisment,
        [Description("Preview <strong>360<sup>o</sup></strong>")] HtmlPreview360,
        [Description("<strong>All</strong> Languages Available")] HtmlAllLanguagesAvailable,
        [Description("Month")] Month,
        [Description("Months")] Months,

        [Description("You can create your own Preview 360 video <sup>o</sup> with Matterport Capture")] Matter1,
        [Description("Create your own mobile presentation (Compatible with iPhone and iPad)")] Matter2,
        [Description("You upload the presentation to your account")] Matter3,
        [Description("Once the download is complete. Copy the link here")] Matter4,
        [Description("Activation")] Activation,
        [Description("Welcome to Eron.gr")] WelcomeMessage0,
        [Description("The Welcome Voucher has just been activated")] WelcomeVoucherMessage1,
        [Description("By Completing the submission of your first Ad will be credited")] WelcomeVoucherMessage2,
        [Description("Thank you")] ThankYou,
        [Description("Land")] Land,
        [Description("New Construction")] NewConstruction,
        [Description("Offer")] Offer,
        [Description("Broker Fee")] BrokerFee,
        [Description("Investment")] Investment,
        [Description("Finance")] Finance,
        [Description("Balance")] Balance,
        [Description("Current Costs")] CurrentCosts,
        [Description("Current Balance")] CurrentBalance,
        [Description("Usage For The Current Month")] UsageForTheCurrentMonth,
        [Description("Prepaid Balance for Ads")] PrepaidBalanceForAds,
        [Description("Refill Balance")] RefillBalance,
        [Description("No Orders To Display")] NoOrdersToDisplay,
        [Description("Orders History")] OrdersHistory,
        [Description("Activation of Premium Ad")] ActivationMessage1,
        [Description("Choose which package you want for your ad.")] ActivationMessage2,



        // Translated

        //[Description("")],
        //[Description("")],
        //[Description("")],
        //[Description("")],
        //[Description("")],
        //[Description("")],
        //[Description("")],
        //[Description("")],
        //[Description("")],





        [Description("A+")] Aplus,
        [Description("A")] A,
        [Description("B+")] Bplus,
        [Description("B")] B,
        [Description("C")] C,
        [Description("D")] D,
        [Description("E")] E,
        [Description("F")] F,
        [Description("G")] G
    }
}

