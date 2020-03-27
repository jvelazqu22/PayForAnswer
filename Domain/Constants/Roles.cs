using System.Collections.Generic;

namespace Domain.Constants
{
    public static class Role
    {
        public const string Admin = "PFA-Admin";
        public const string CampaignManager = "PFA-CampaignManager";
        public const string PayPal = "PayPal";
        public const string PayForAnswer = "PayForAnswer";

        static public List<string> UserRoleList = new List<string>() { Admin, CampaignManager };
        static public List<string> CmAndAdminRolesList = new List<string>() { Admin, CampaignManager };
        public const string CommaSeparateCMandAdminRoles = Admin + "," + CampaignManager;
    }

}
