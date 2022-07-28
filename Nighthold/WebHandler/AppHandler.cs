using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebHandler
{

    /*------------------------------------------------------------------------------------------------
     --------------------------------------- CI_SESSIONS CLASS ---------------------------------------
     ------------------------------------------------------------------------------------------------*/
    public class CiSessionsClass
    {
        #region ACTIVE SESSIONS LIST
        public partial class ActiveSessionsList
        {
            [JsonProperty("avatar_url")]
            public string AvatarUrl { get; set; }

            [JsonProperty("account_id")]
            public long AccountId { get; set; }

            [JsonProperty("account_name")]
            public string AccountName { get; set; }

            [JsonProperty("last_session_id")]
            public string LastSessionId { get; set; }

            [JsonProperty("last_ip")]
            public string LastIp { get; set; }

            [JsonProperty("last_seen")]
            public DateTimeOffset LastSeen { get; set; }
        }

        public partial class ActiveSessionsList
        {
            public static List<ActiveSessionsList> FromJson(string json) => JsonConvert.DeserializeObject<List<ActiveSessionsList>>(json, Converter.Settings);
        }

        
        public static async Task<string> GetActiveSessionsListJson(string username, string password, string md5secpa)
        {
            return await WebTool.GetStringFromPOST(Config.AppUrl, new Dictionary<string, string>
            {
                { "type", "ci_active_sessions_list" },
                { "user", username },
                { "pass", password },
                { "md5secpa", md5secpa }
            });
        }
        #endregion
        #region PING ME ALIVE
        public static async Task PingMeAlive(string username)
        {
            await WebTool.SendJsonPOST(Config.AppUrl, new Dictionary<string, string>
            {
                { "type", "ping_me_alive" },
                { "user", username }
            });
        }
        #endregion
    }

    /*------------------------------------------------------------------------------------------------
     ----------------------------------------- NEWS CLASS --------------------------------------------
     ------------------------------------------------------------------------------------------------*/
    public class NewsClass
    {
        #region NEWS LIST
        public partial class NewsList
        {
            [JsonProperty("articleId")]
            public long ArticleId { get; set; }

            [JsonProperty("articleTitle")]
            public string ArticleTitle { get; set; }

            [JsonProperty("articleDate")]
            public DateTimeOffset ArticleDate { get; set; }

            [JsonProperty("articleUrl")]
            public string ArticleUrl { get; set; }

            [JsonProperty("imageUrl")]
            public string ImageUrl { get; set; }
        }


        public partial class NewsList
        {
            public static List<NewsList> FromJson(string json) => JsonConvert.DeserializeObject<List<NewsList>>(json, Converter.Settings);
        }

        public static async Task<string> GetNewsListJson(string expansionId, string limit)
        {
            return await WebTool.GetStringFromPOST(Config.AppUrl, new Dictionary<string, string>
            {
                { "type", "news_list" },
                { "expansionid", expansionId },
                { "limit", limit }
            });
        }
        #endregion
        #region ARTICLE CREATE
        public partial class ArticleCreate
        {
            [JsonProperty("responseMsg")]
            public string ResponseMsg { get; set; }
        }

        public partial class ArticleCreate
        {
            public static ArticleCreate FromJson(string json) => JsonConvert.DeserializeObject<ArticleCreate>(json, Converter.Settings);
        }

        public static async Task<string> GetArticleCreateResponseJson(string username, string password, string md5secpa,
            string expansionId, string newTitle, string newArticleUrl, string newImageUrl)
        {
            return await WebTool.GetStringFromPOST(Config.AppUrl, new Dictionary<string, string>
            {
                { "type", "news_article_create" },
                { "user", username },
                { "pass", password },
                { "md5secpa", md5secpa },
                { "expansionid", expansionId },
                { "newtitle", newTitle },
                { "newarticleurl", newArticleUrl },
                { "newimageurl", newImageUrl }
            });
        }
        #endregion
        #region ARTICLE EDIT
        public partial class ArticleEdit
        {
            [JsonProperty("responseMsg")]
            public string ResponseMsg { get; set; }
        }

        public partial class ArticleEdit
        {
            public static ArticleEdit FromJson(string json) => JsonConvert.DeserializeObject<ArticleEdit>(json, Converter.Settings);
        }

        public static async Task<string> GetArticleEditResponseJson(string username, string password, string md5secpa,
            string articleId, string expansionId, string newTitle, string newArticleUrl, string newImageUrl)
        {
            return await WebTool.GetStringFromPOST(Config.AppUrl, new Dictionary<string, string>
            {
                { "type", "news_article_edit" },
                { "user", username },
                { "pass", password },
                { "md5secpa", md5secpa },
                { "articleid", articleId },
                { "expansionid", expansionId },
                { "newtitle", newTitle },
                { "newarticleurl", newArticleUrl },
                { "newimageurl", newImageUrl }
            });
        }
        #endregion
        #region ARTICLE DELETE
        public partial class ArticleDelete
        {
            [JsonProperty("responseMsg")]
            public string ResponseMsg { get; set; }
        }

        public partial class ArticleDelete
        {
            public static ArticleDelete FromJson(string json) => JsonConvert.DeserializeObject<ArticleDelete>(json, Converter.Settings);
        }

        public static async Task<string> GetArticleDeleteResponseJson(string username, string password, string md5secpa, string articleId, string expansionId)
        {
            return await WebTool.GetStringFromPOST(Config.AppUrl, new Dictionary<string, string>
            {
                { "type", "news_article_delete" },
                { "user", username },
                { "pass", password },
                { "md5secpa", md5secpa },
                { "articleid", articleId },
                { "expansionid", expansionId }
            });
        }
        #endregion
    }

    /*------------------------------------------------------------------------------------------------
     -------------------------------------- NOTIFICATIONS CLASS --------------------------------------
     ------------------------------------------------------------------------------------------------*/
    public class NotificationsClass
    {
        #region NOTIFICATIONS LIST
        public partial class NotificationsList
        {
            [JsonProperty("id")]
            public long Id { get; set; }

            [JsonProperty("subject")]
            public string Subject { get; set; }

            [JsonProperty("message")]
            public string Message { get; set; }

            [JsonProperty("imageUrl")]
            public string ImageUrl { get; set; }

            [JsonProperty("redirectUrl")]
            public string RedirectUrl { get; set; }

            [JsonProperty("isMarkedAsRead")]
            public bool IsMarkedAsRead { get; set; }
        }

        public partial class NotificationsList
        {
            public static List<NotificationsList> FromJson(string json) => JsonConvert.DeserializeObject<List<NotificationsList>>(json, Converter.Settings);
        }

        public static async Task<string> GetNotificationsListJson(string username, string password)
        {
            return await WebTool.GetStringFromPOST(Config.AppUrl, new Dictionary<string, string>
            {
                { "type", "notifications_list" },
                { "user", username },
                { "pass", password }
            });
        }
        #endregion
        #region NOTIFICATION MARK AS READ
        public static async Task MarkNotificationAsRead(string username, string password, string notificationId)
        {
            await WebTool.SendJsonPOST(Config.AppUrl, new Dictionary<string, string>
            {
                { "type", "notification_mark_as_read" },
                { "user", username },
                { "pass", password },
                { "notificationid", notificationId }
            });
        }
        #endregion
        #region NOTIFICATIONS LIST AS ADMIN
        public partial class NotificationsListAsAdmin
        {
            [JsonProperty("id")]
            public long Id { get; set; }

            [JsonProperty("subject")]
            public string Subject { get; set; }

            [JsonProperty("message")]
            public string Message { get; set; }

            [JsonProperty("imageUrl")]
            public string ImageUrl { get; set; }

            [JsonProperty("redirectUrl")]
            public string RedirectUrl { get; set; }

            [JsonProperty("mention")]
            public string Mention { get; set; }
        }

        public partial class NotificationsListAsAdmin
        {
            public static List<NotificationsListAsAdmin> FromJson(string json) => JsonConvert.DeserializeObject<List<NotificationsListAsAdmin>>(json, Converter.Settings);
        }

        public static async Task<string> GetNotificationsListAsAdminJson(string username, string password, string md5secpa)
        {
            return await WebTool.GetStringFromPOST(Config.AppUrl, new Dictionary<string, string>
            {
                { "type", "notifications_list_as_admin" },
                { "user", username },
                { "pass", password },
                { "md5secpa", md5secpa }
            });
        }
        #endregion
        #region NOTIFICATION CREATE
        public partial class NotificationCreate
        {
            [JsonProperty("responseMsg")]
            public string ResponseMsg { get; set; }
        }

        public partial class NotificationCreate
        {
            public static NotificationCreate FromJson(string json) => JsonConvert.DeserializeObject<NotificationCreate>(json, Converter.Settings);
        }

        public static async Task<string> GetNotificationCreateResponseJson(string username, string password, string md5secpa,
            string newsubject, string newmessage, string newimageurl, string newredirecturl, string newaccountid)
        {
            return await WebTool.GetStringFromPOST(Config.AppUrl, new Dictionary<string, string>
            {
                { "type", "notification_create" },
                { "user", username },
                { "pass", password },
                { "md5secpa", md5secpa },
                { "newsubject", newsubject },
                { "newmessage", newmessage },
                { "newimageurl", newimageurl },
                { "newredirecturl", newredirecturl },
                { "newaccountid", newaccountid }
            });
        }
        #endregion
        #region NOTIFICATION DELETE
        public partial class NotificationDelete
        {
            [JsonProperty("responseMsg")]
            public string ResponseMsg { get; set; }
        }

        public partial class NotificationDelete
        {
            public static NotificationDelete FromJson(string json) => JsonConvert.DeserializeObject<NotificationDelete>(json, Converter.Settings);
        }

        public static async Task<string> GetNotificationDeleteResponseJson(string username, string password, string md5secpa, string notificationid)
        {
            return await WebTool.GetStringFromPOST(Config.AppUrl, new Dictionary<string, string>
            {
                { "type", "notification_delete" },
                { "user", username },
                { "pass", password },
                { "md5secpa", md5secpa },
                { "notificationid", notificationid }
            });
        }
        #endregion
    }

    /*------------------------------------------------------------------------------------------------
     --------------------------------------- DISCORD CLASS -------------------------------------------
     ------------------------------------------------------------------------------------------------*/
    public class DiscordClass
    {
        #region DISCORD ISSUE REPORT
        public static async Task SendNewIssueReport(string reportBy, string launcherVersion, string issueAt, string issueMessage)
        {
            await WebTool.SendJsonPOST(Config.AppUrl, new Dictionary<string, string>
            {
                { "type", "discord_issue_report" },
                { "by", reportBy },
                { "version", launcherVersion },
                { "at", issueAt },
                { "issue", issueMessage }
            });
        }
        #endregion
    }

    /*------------------------------------------------------------------------------------------------
     ---------------------------------------- AUTH CLASS ---------------------------------------------
     ------------------------------------------------------------------------------------------------*/
    public class AuthClass
    {
        #region LOGIN RESPONSE
        public partial class LoginResponse
        {
            [JsonProperty("username")]
            public string Username { get; set; }

            [JsonProperty("response")]
            public string Response { get; set; }

            [JsonProperty("logged")]
            public bool Logged { get; set; }
        }

        public partial class LoginResponse
        {
            public static LoginResponse FromJson(string json) => JsonConvert.DeserializeObject<LoginResponse>(json, Converter.Settings);
        }

        public static async Task<string> GetLoginReponseJson(string username, string password)
        {
            return await WebTool.GetStringFromPOST(Config.AppUrl, new Dictionary<string, string>
            {
                { "type", "login_response" },
                { "user", username },
                { "pass", password }
            });
        }
        #endregion
        #region SEC PA RESPONSE
        public partial class SecPaResponse
        {
            [JsonProperty("response")]
            public bool Response { get; set; }
        }

        public partial class SecPaResponse
        {
            public static SecPaResponse FromJson(string json) => JsonConvert.DeserializeObject<SecPaResponse>(json, Converter.Settings);
        }

        public static async Task<string> GetSecPResponseJson(string username, string password, string md5secpa)
        {
            return await WebTool.GetStringFromPOST(Config.AppUrl, new Dictionary<string, string>
            {
                { "type", "secpa_response" },
                { "user", username },
                { "pass", password },
                { "md5secpa", md5secpa }
            });
        }
        #endregion
        #region ACCOUNT RANK NAME
        public partial class AccountRankName
        {
            [JsonProperty("rankColor")]
            public string RankColor { get; set; }

            [JsonProperty("rankName")]
            public string RankName { get; set; }
        }

        public partial class AccountRankName
        {
            public static AccountRankName FromJson(string json) => JsonConvert.DeserializeObject<AccountRankName>(json, Converter.Settings);
        }

        public static async Task<string> GetAccountRankNameJson(string username, string password)
        {
            return await WebTool.GetStringFromPOST(Config.AppUrl, new Dictionary<string, string>
            {
                { "type", "account_rank_name" },
                { "user", username },
                { "pass", password }
            });
        }
        #endregion
        #region ACCOUNT STANDING
        public partial class AccountStanding
        {
            [JsonProperty("standing")]
            public string Standing { get; set; }

            [JsonProperty("banTimeLeft")]
            public string BanTineLeft { get; set; }
        }

        public partial class AccountStanding
        {
            public static AccountStanding FromJson(string json) => JsonConvert.DeserializeObject<AccountStanding>(json, Converter.Settings);
        }

        public static async Task<string> GetAccountStandingJson(string username, string password)
        {
            return await WebTool.GetStringFromPOST(Config.AppUrl, new Dictionary<string, string>
            {
                { "type", "account_standing" },
                { "user", username },
                { "pass", password }
            });
        }
        #endregion
        #region ACCOUNT BALANCE
        public partial class AccountBalance
        {
            [JsonProperty("dp")]
            public long DP { get; set; }

            [JsonProperty("vp")]
            public long VP { get; set; }
        }

        public partial class AccountBalance
        {
            public static List<AccountBalance> FromJson(string json) => JsonConvert.DeserializeObject<List<AccountBalance>>(json, Converter.Settings);
        }

        public static async Task<string> GetAccountBalanceJson(string username, string password)
        {
            return await WebTool.GetStringFromPOST(Config.AppUrl, new Dictionary<string, string>
            {
                { "type", "account_balance" },
                { "user", username },
                { "pass", password }
            });
        }
        #endregion
        #region GM PANEL ACCESS
        public partial class GMPanelAccess
        {
            [JsonProperty("canAccess")]
            public bool CanAccess { get; set; }
        }

        public partial class GMPanelAccess
        {
            public static GMPanelAccess FromJson(string json) => JsonConvert.DeserializeObject<GMPanelAccess>(json, Converter.Settings);
        }

        public static async Task<string> GetGMPanelAccessJson(string username, string password)
        {
            return await WebTool.GetStringFromPOST(Config.AppUrl, new Dictionary<string, string>
            {
                { "type", "gmpanel_access" },
                { "user", username },
                { "pass", password }
            });
        }
        #endregion
        #region ADMIN PANEL ACCESS
        public partial class AdminPanelAccess
        {
            [JsonProperty("canAccess")]
            public bool CanAccess { get; set; }
        }

        public partial class AdminPanelAccess
        {
            public static AdminPanelAccess FromJson(string json) => JsonConvert.DeserializeObject<AdminPanelAccess>(json, Converter.Settings);
        }

        public static async Task<string> GetAdminPanelAccessJson(string username, string password)
        {
            return await WebTool.GetStringFromPOST(Config.AppUrl, new Dictionary<string, string>
            {
                { "type", "adminpanel_access" },
                { "user", username },
                { "pass", password }
            });
        }
        #endregion
        #region DB AVATARS LIST
        public partial class DBAvatarsList
        {
            [JsonProperty("id")]
            public long Id { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("url")]
            public string Url { get; set; }
        }

        public partial class DBAvatarsList
        {
            public static List<DBAvatarsList> FromJson(string json) => JsonConvert.DeserializeObject<List<DBAvatarsList>>(json, Converter.Settings);
        }

        public static async Task<string> GetDBAvatarsListJson(string username, string password)
        {
            return await WebTool.GetStringFromPOST(Config.AppUrl, new Dictionary<string, string>
            {
                { "type", "db_avatars_list" },
                { "user", username },
                { "pass", password }
            });
        }
        #endregion
        #region GET SELF AVATAR
        public partial class SelfAvatarUrl
        {
            [JsonProperty("avatarUrl")]
            public string AvatarUrl { get; set; }
        }

        public partial class SelfAvatarUrl
        {
            public static SelfAvatarUrl FromJson(string json) => JsonConvert.DeserializeObject<SelfAvatarUrl>(json, Converter.Settings);
        }

        public static async Task<string> GetSelfAvatarUrlJson(string username, string password)
        {
            return await WebTool.GetStringFromPOST(Config.AppUrl, new Dictionary<string, string>
            {
                { "type", "get_self_avatar" },
                { "user", username },
                { "pass", password }
            });
        }
        #endregion
        #region SET SELF AVATAR
        public static async Task SetSelfAvatar(string username, string password, string dbAvatarUrl)
        {
            await WebTool.SendJsonPOST(Config.AppUrl, new Dictionary<string, string>
            {
                { "type", "set_self_avatar" },
                { "user", username },
                { "pass", password },
                { "db_avatar_url", dbAvatarUrl }
            });
        }
        #endregion
        #region VOTE SITES LIST
        public partial class VoteSitesList
        {
            [JsonProperty("siteID")]
            public long SiteId { get; set; }

            [JsonProperty("siteName")]
            public string SiteName { get; set; }

            [JsonProperty("cooldownSecLeft")]
            public long CooldownSecLeft { get; set; }

            [JsonProperty("imageUrl")]
            public string ImageUrl { get; set; }

            [JsonProperty("voteUrl")]
            public string VoteUrl { get; set; }

            [JsonProperty("points")]
            public long Points { get; set; }
        }

        public partial class VoteSitesList
        {
            public static List<VoteSitesList> FromJson(string json) => JsonConvert.DeserializeObject<List<VoteSitesList>>(json, Converter.Settings);
        }

        public static async Task<string> GetVoteSitesListJson(string username, string password)
        {
            return await WebTool.GetStringFromPOST(Config.AppUrl, new Dictionary<string, string>
            {
                { "type", "vote_sites_list" },
                { "user", username },
                { "pass", password }
            });
        }
        #endregion
        #region SELF VOTE CLICK
        public partial class VoteClickResponse
        {
            [JsonProperty("responseMsg")]
            public string ResponseMsg { get; set; }

            [JsonProperty("voteRegistered")]
            public bool VoteRegistered { get; set; }
        }

        public partial class VoteClickResponse
        {
            public static VoteClickResponse FromJson(string json) => JsonConvert.DeserializeObject<VoteClickResponse>(json, Converter.Settings);
        }

        public static async Task<string> SelfVoteClick(string username, string password, string siteId)
        {
            return await WebTool.GetStringFromPOST(Config.AppUrl, new Dictionary<string, string>
            {
                { "type", "self_vote_click" },
                { "user", username },
                { "pass", password },
                { "siteid", siteId }
            });
        }
        #endregion
        #region SHOP LIST
        public partial class ShopList
        {
            [JsonProperty("id")]
            public long Id { get; set; }

            [JsonProperty("title")]
            public string Title { get; set; }

            [JsonProperty("description")]
            public string Description { get; set; }

            [JsonProperty("img_url")]
            public string ImgUrl { get; set; }

            [JsonProperty("price_dp")]
            public long PriceDp { get; set; }

            [JsonProperty("price_vp")]
            public long PriceVp { get; set; }

            [JsonProperty("category")]
            public long Category { get; set; }

            [JsonProperty("realmid")]
            public long RealmId { get; set; }
        }

        public partial class ShopList
        {
            public static List<ShopList> FromJson(string json) => JsonConvert.DeserializeObject<List<ShopList>>(json, Converter.Settings);
        }

        public static async Task<string> GetShopListJson(string username, string password)
        {
            return await WebTool.GetStringFromPOST(Config.AppUrl, new Dictionary<string, string>
            {
                { "type", "shop_list" },
                { "user", username },
                { "pass", password }
            });
        }
        #endregion
        #region SHOP PURCHASE
        public partial class ShopPurchaseResponse
        {
            [JsonProperty("responseMsg")]
            public string ResponseMsg { get; set; }

            [JsonProperty("response")]
            public bool Response { get; set; }
        }

        public partial class ShopPurchaseResponse
        {
            public static ShopPurchaseResponse FromJson(string json) => JsonConvert.DeserializeObject<ShopPurchaseResponse>(json, Converter.Settings);
        }

        public static async Task<string> GetShopPurchaseResponse(string username, string password, string id, string currencyType, string playerName, string accountName)
        {
            return await WebTool.GetStringFromPOST(Config.AppUrl, new Dictionary<string, string>
            {
                { "type", "shop_purchase" },
                { "user", username },
                { "pass", password },
                { "id", id },
                { "currencyType", currencyType },
                { "playerName", playerName },
                { "accountName", accountName }
            });
        }
        #endregion
    }

    /*------------------------------------------------------------------------------------------------
     ---------------------------------- CHARARACTERS MARKET CLASS ------------------------------------
     ------------------------------------------------------------------------------------------------*/
    public class CharactersMarketClass
    {
        #region CHARACTERS MARKET LIST
        public partial class CharactersMarketList
        {
            [JsonProperty("market_id")]
            public long MarketId { get; set; }

            [JsonProperty("guid")]
            public long Guid { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("race")]
            public long Race { get; set; }

            [JsonProperty("class")]
            public long Class { get; set; }

            [JsonProperty("gender")]
            public long Gender { get; set; }

            [JsonProperty("level")]
            public long Level { get; set; }

            [JsonProperty("price_dp")]
            public long PriceDp { get; set; }

            [JsonProperty("realm_id")]
            public long RealmId { get; set; }

            [JsonProperty("realm_name")]
            public string RealmName { get; set; }
        }

        public partial class CharactersMarketList
        {
            public static List<CharactersMarketList> FromJson(string json) => JsonConvert.DeserializeObject<List<CharactersMarketList>>(json, Converter.Settings);
        }

        public static async Task<string> GetCharactersMarketListJson(string username, string password)
        {
            return await WebTool.GetStringFromPOST(Config.AppUrl, new Dictionary<string, string>
            {
                { "type", "characters_market_list" },
                { "user", username },
                { "pass", password }
            });
        }
        #endregion
        #region CHARACTERS MARKET OWN LIST
        public partial class CharactersMarketOwnList
        {
            [JsonProperty("market_id")]
            public long MarketId { get; set; }

            [JsonProperty("guid")]
            public long Guid { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("race")]
            public long Race { get; set; }

            [JsonProperty("class")]
            public long Class { get; set; }

            [JsonProperty("gender")]
            public long Gender { get; set; }

            [JsonProperty("level")]
            public long Level { get; set; }

            [JsonProperty("price_dp")]
            public long PriceDp { get; set; }

            [JsonProperty("realm_id")]
            public long RealmId { get; set; }

            [JsonProperty("realm_name")]
            public string RealmName { get; set; }
        }

        public partial class CharactersMarketOwnList
        {
            public static List<CharactersMarketOwnList> FromJson(string json) => JsonConvert.DeserializeObject<List<CharactersMarketOwnList>>(json, Converter.Settings);
        }

        public static async Task<string> GetCharactersMarketOwnListJson(string username, string password)
        {
            return await WebTool.GetStringFromPOST(Config.AppUrl, new Dictionary<string, string>
            {
                { "type", "characters_market_own_list" },
                { "user", username },
                { "pass", password }
            });
        }
        #endregion
        #region MARKET PURCHASE
        public partial class MarketPurchaseResponse
        {
            [JsonProperty("responseMsg")]
            public string ResponseMsg { get; set; }

            [JsonProperty("response")]
            public bool Response { get; set; }
        }

        public partial class MarketPurchaseResponse
        {
            public static MarketPurchaseResponse FromJson(string json) => JsonConvert.DeserializeObject<MarketPurchaseResponse>(json, Converter.Settings);
        }

        public static async Task<string> GetMarketPurchaseResponse(string marketId, string username, string password)
        {
            return await WebTool.GetStringFromPOST(Config.AppUrl, new Dictionary<string, string>
            {
                { "type", "characters_market_purchase_id" },
                { "market_id", marketId },
                { "user", username },
                { "pass", password }
            });
        }
        #endregion
        #region CHARACTERS LIST
        public partial class OwnCharactersList
        {
            [JsonProperty("guid")]
            public long Guid { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("class")]
            public long Class { get; set; }

            [JsonProperty("race")]
            public long Race { get; set; }

            [JsonProperty("gender")]
            public long Gender { get; set; }

            [JsonProperty("level")]
            public long Level { get; set; }

            [JsonProperty("realm_id")]
            public long RealmId { get; set; }

            [JsonProperty("realm_name")]
            public string RealmName { get; set; }
        }

        public partial class OwnCharactersList
        {
            public static List<List<OwnCharactersList>> FromJson(string json) => JsonConvert.DeserializeObject<List<List<OwnCharactersList>>>(json, Converter.Settings);
        }

        public static async Task<string> GetOwnCharactersListJson(string username, string password)
        {
            return await WebTool.GetStringFromPOST(Config.AppUrl, new Dictionary<string, string>
            {
                { "type", "characters_market_own_characters_list" },
                { "user", username },
                { "pass", password }
            });
        }
        #endregion
        #region MARKET SELL
        public partial class MarketSellResponse
        {
            [JsonProperty("responseMsg")]
            public string ResponseMsg { get; set; }

            [JsonProperty("response")]
            public bool Response { get; set; }
        }

        public partial class MarketSellResponse
        {
            public static MarketSellResponse FromJson(string json) => JsonConvert.DeserializeObject<MarketSellResponse>(json, Converter.Settings);
        }

        public static async Task<string> GetMarketSellResponse(string username, string password, string guid, string priceDP, string realmID)
        {
            return await WebTool.GetStringFromPOST(Config.AppUrl, new Dictionary<string, string>
            {
                { "type", "characters_market_sell_character" },
                { "user", username },
                { "pass", password },
                { "guid", guid },
                { "price_dp", priceDP },
                { "realm_id", realmID }
            });
        }
        #endregion
        #region MARKET CANCEL
        public partial class MarketCancelSaleResponse
        {
            [JsonProperty("responseMsg")]
            public string ResponseMsg { get; set; }

            [JsonProperty("response")]
            public bool Response { get; set; }
        }

        public partial class MarketCancelSaleResponse
        {
            public static MarketCancelSaleResponse FromJson(string json) => JsonConvert.DeserializeObject<MarketCancelSaleResponse>(json, Converter.Settings);
        }

        public static async Task<string> GetMarketCancelSaleResponse(string username, string password, string guid, string realmID)
        {
            return await WebTool.GetStringFromPOST(Config.AppUrl, new Dictionary<string, string>
            {
                { "type", "characters_market_cancel_sale" },
                { "user", username },
                { "pass", password },
                { "guid", guid },
                { "realm_id", realmID }
            });
        }
        #endregion
    }

    /*------------------------------------------------------------------------------------------------
     ---------------------------------------- CHAR CLASS ---------------------------------------------
     ------------------------------------------------------------------------------------------------*/
    public class CharClass
    {
        #region CHARACTERS LIST
        public partial class CharacterData
        {
            [JsonProperty("realm")]
            public string Realm { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("gender")]
            [JsonConverter(typeof(ParseStringConverter))]
            public long Gender { get; set; }

            [JsonProperty("level")]
            [JsonConverter(typeof(ParseStringConverter))]
            public long Level { get; set; }

            [JsonProperty("race")]
            [JsonConverter(typeof(ParseStringConverter))]
            public long Race { get; set; }

            [JsonProperty("class")]
            [JsonConverter(typeof(ParseStringConverter))]
            public long Class { get; set; }
        }

        public partial class CharacterData
        {
            public static List<List<CharacterData>> FromJson(string json) => JsonConvert.DeserializeObject<List<List<CharacterData>>>(json, Converter.Settings);
        }

        public static async Task<string> GetCharactersListJson(string username, string password)
        {
            return await WebTool.GetStringFromPOST(Config.AppUrl, new Dictionary<string, string>
            {
                { "type", "characters_list" },
                { "user", username },
                { "pass", password }
            });
        }
        #endregion
        #region REALM CHARACTERS LIST
        public partial class RealmCharacterData
        {
            [JsonProperty("realm")]
            public string Realm { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("gender")]
            [JsonConverter(typeof(ParseStringConverter))]
            public long Gender { get; set; }

            [JsonProperty("level")]
            [JsonConverter(typeof(ParseStringConverter))]
            public long Level { get; set; }

            [JsonProperty("race")]
            [JsonConverter(typeof(ParseStringConverter))]
            public long Race { get; set; }

            [JsonProperty("class")]
            [JsonConverter(typeof(ParseStringConverter))]
            public long Class { get; set; }
        }

        public partial class RealmCharacterData
        {
            public static List<List<RealmCharacterData>> FromJson(string json) => JsonConvert.DeserializeObject<List<List<RealmCharacterData>>>(json, Converter.Settings);
        }

        public static async Task<string> GetRealmCharactersListJson(string username, string password, string realmid)
        {
            return await WebTool.GetStringFromPOST(Config.AppUrl, new Dictionary<string, string>
            {
                { "type", "realm_characters_list" },
                { "user", username },
                { "pass", password },
                { "realmid", realmid }
            });
        }
        #endregion
        #region TOP PVP LIST
        public partial class TopPvPList
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("arenaPoints")]
            public long ArenaPoints { get; set; }

            [JsonProperty("totalHonorPoints")]
            public long TotalHonorPoints { get; set; }

            [JsonProperty("totalKills")]
            public long TotalKills { get; set; }

            [JsonProperty("realm")]
            public string Realm { get; set; }
        }

        public partial class TopPvPList
        {
            public static List<List<TopPvPList>> FromJson(string json) => JsonConvert.DeserializeObject<List<List<TopPvPList>>>(json, Converter.Settings);
        }

        public static async Task<string> GetTopPvPListJson(string limit)
        {
            return await WebTool.GetStringFromPOST(Config.AppUrl, new Dictionary<string, string>
            {
                { "type", "top_pvp_list" },
                { "limit", limit }
            });
        }
        #endregion

        //delete  patch

        public partial class DeleteOldPatchesList
        {
            [JsonProperty("patch")]
            public string Patch { get; set; }
        }

        public partial class DeleteOldPatchesList
        {
            public static List<List<DeleteOldPatchesList>> FromJson(string json) => JsonConvert.DeserializeObject<List<List<DeleteOldPatchesList>>>(json, Converter.Settings);
        }

        public static async Task<string> GetDeletePatchListJson()
        {
            return await WebTool.GetStringFromPOST(Config.AppUrl, new Dictionary<string, string>
            {
                { "type", "delete_patch_list" }
            });
        }

        #region ONLINE PLAYERS LIST
        public partial class OnlinePlayersList
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("level")]
            public long Level { get; set; }

            [JsonProperty("race")]
            public long Race { get; set; }

            [JsonProperty("gender")]
            public long Gender { get; set; }

            [JsonProperty("class")]
            public long Class { get; set; }

            [JsonProperty("realm")]
            public string Realm { get; set; }
        }

        public partial class OnlinePlayersList
        {
            public static List<List<OnlinePlayersList>> FromJson(string json) => JsonConvert.DeserializeObject<List<List<OnlinePlayersList>>>(json, Converter.Settings);
        }

        public static async Task<string> GetOnlinePlayersListJson()
        {
            return await WebTool.GetStringFromPOST(Config.AppUrl, new Dictionary<string, string>
            {
                { "type", "online_list" }
            });
        }
        #endregion
    }

    /*------------------------------------------------------------------------------------------------
     ------------------------------------ GAME MASTER CLASS ------------------------------------------
     ------------------------------------------------------------------------------------------------*/
    public class GameMasterClass
    {
        #region TICKETS LIST
        public partial class Tickets
        {
            [JsonProperty("ticketId")]
            public long TicketId { get; set; }

            [JsonProperty("ticketName")]
            public string TicketName { get; set; }

            [JsonProperty("ticketOnline")]
            public long TicketOnline { get; set; }

            [JsonProperty("ticketRace")]
            public long TicketRace { get; set; }

            [JsonProperty("ticketClass")]
            public long TicketClass { get; set; }

            [JsonProperty("ticketGender")]
            public long TicketGender { get; set; }

            [JsonProperty("ticketMessage")]
            public string TicketMessage { get; set; }

            [JsonProperty("ticketCreateTime")]
            public long TicketCreateTime { get; set; }

            [JsonProperty("ticketLastModified")]
            public long TicketLastModified { get; set; }

            [JsonProperty("ticketAssignedTo")]
            public string TicketAssignedTo { get; set; }

            [JsonProperty("ticketComment")]
            public string TicketComment { get; set; }

            [JsonProperty("ticketRealmName")]
            public string TicketRealmName { get; set; }

            [JsonProperty("ticketRealmId")]
            public long TicketRealmId { get; set; }
        }

        public partial class Tickets
        {
            public static List<Tickets> FromJson(string json) => JsonConvert.DeserializeObject<List<Tickets>>(json, Converter.Settings);
        }

        public static async Task<string> GetTicketsListJson(string username, string password)
        {
            return await WebTool.GetStringFromPOST(Config.AppUrl, new Dictionary<string, string>
            {
                { "type", "tickets_list" },
                { "user", username },
                { "pass", password }
            });
        }
        #endregion
        #region BANS LIST
        public partial class AllBansList
        {
            [JsonProperty("banType")]
            public long BanType { get; set; }

            [JsonProperty("accOrCharName")]
            public string AccOrCharName { get; set; }

            [JsonProperty("banDate")]
            public long BanDate { get; set; }

            [JsonProperty("unbanDate")]
            public long UnbanDate { get; set; }

            [JsonProperty("bannedBy")]
            public string BannedBy { get; set; }

            [JsonProperty("banReason")]
            public string BanReason { get; set; }

            [JsonProperty("realmName", NullValueHandling = NullValueHandling.Ignore)]
            public string RealmName { get; set; }

            [JsonProperty("realmId", NullValueHandling = NullValueHandling.Ignore)]
            public long? RealmId { get; set; }
        }

        public partial class AllBansList
        {
            public static List<List<AllBansList>> FromJson(string json) => JsonConvert.DeserializeObject<List<List<AllBansList>>>(json, Converter.Settings);
        }

        public static async Task<string> GetAllBansListJson(string username, string password)
        {
            return await WebTool.GetStringFromPOST(Config.AppUrl, new Dictionary<string, string>
            {
                { "type", "all_bans_list" },
                { "user", username },
                { "pass", password }
            });
        }
        #endregion
        #region MUTE LOGS
        public partial class MuteLogs
        {
            [JsonProperty("username")]
            public string Username { get; set; }

            [JsonProperty("muteDate")]
            public long MuteDate { get; set; }

            [JsonProperty("unmuteDate")]
            public long UnmuteDate { get; set; }

            [JsonProperty("muteTime")]
            public long MuteTime { get; set; }

            [JsonProperty("mutedBy")]
            public string MutedBy { get; set; }

            [JsonProperty("muteReason")]
            public string MuteReason { get; set; }
        }

        public partial class MuteLogs
        {
            public static List<MuteLogs> FromJson(string json) => JsonConvert.DeserializeObject<List<MuteLogs>>(json, Converter.Settings);
        }
        public static async Task<string> GetMuteLogsJson(string username, string password)
        {
            return await WebTool.GetStringFromPOST(Config.AppUrl, new Dictionary<string, string>
            {
                { "type", "mute_logs" },
                { "user", username },
                { "pass", password }
            });
        }
        #endregion
        #region SHOP LOGS
        public partial class Shoplogs
        {
            [JsonProperty("accountName")]
            public string AccountName { get; set; }

            [JsonProperty("playerName")]
            public string PlayerName { get; set; }

            [JsonProperty("itemId")]
            public long ItemId { get; set; }

            [JsonProperty("amount")]
            public long Amount { get; set; }

            [JsonProperty("price")]
            public long Price { get; set; }

            [JsonProperty("points")]
            public string Points { get; set; }

            [JsonProperty("date")]
            public DateTimeOffset Date { get; set; }

            [JsonProperty("realmName")]
            public string RealmName { get; set; }
        }

        public partial class Shoplogs
        {
            public static List<Shoplogs> FromJson(string json) => JsonConvert.DeserializeObject<List<Shoplogs>>(json, Converter.Settings);
        }
        public static async Task<string> GetShopLogsJson(string username, string password)
        {
            return await WebTool.GetStringFromPOST(Config.AppUrl, new Dictionary<string, string>
            {
                { "type", "shop_logs" },
                { "user", username },
                { "pass", password }
            });
        }
        #endregion
        #region COMMANDS LOGS
        public partial class CommandLogs
        {
            [JsonProperty("dateAndTime")]
            public string DateAndTime { get; set; }

            [JsonProperty("realmName")]
            public string RealmName { get; set; }

            [JsonProperty("level")]
            public long Level { get; set; }

            [JsonProperty("command")]
            public string Command { get; set; }
        }

        public partial class CommandLogs
        {
            public static List<CommandLogs> FromJson(string json) => JsonConvert.DeserializeObject<List<CommandLogs>>(json, Converter.Settings);
        }

        public static async Task<string> GetCommandLogsJson(string username, string password, string md5secpa)
        {
            return await WebTool.GetStringFromPOST(Config.AppUrl, new Dictionary<string, string>
            {
                { "type", "command_logs" },
                { "user", username },
                { "pass", password },
                { "md5secpa", md5secpa }
            });
        }
        #endregion
        #region SOAP LOGS
        public partial class SoapLogs
        {
            [JsonProperty("id")]
            public long Id { get; set; }

            [JsonProperty("accountId")]
            public long AccountId { get; set; }

            [JsonProperty("accountName")]
            public string AccountName { get; set; }

            [JsonProperty("date")]
            public DateTimeOffset Date { get; set; }

            [JsonProperty("realmName")]
            public string RealmName { get; set; }

            [JsonProperty("command")]
            public string Command { get; set; }
        }

        public partial class SoapLogs
        {
            public static List<SoapLogs> FromJson(string json) => JsonConvert.DeserializeObject<List<SoapLogs>>(json, Converter.Settings);
        }

        public static async Task<string> GetSoapLogsJson(string username, string password, string md5secpa)
        {
            return await WebTool.GetStringFromPOST(Config.AppUrl, new Dictionary<string, string>
            {
                { "type", "soap_logs" },
                { "user", username },
                { "pass", password },
                { "md5secpa", md5secpa }
            });
        }
        #endregion
        #region PLAYER INFO
        public partial class PlayerInfo
        {
            [JsonProperty("accountName")]
            public string AccountName { get; set; }

            [JsonProperty("accountId")]
            public long AccountId { get; set; }

            [JsonProperty("accountRankColor")]
            public string AccountRankColor { get; set; }

            [JsonProperty("accountRankName")]
            public string AccountRankName { get; set; }

            [JsonProperty("playerName")]
            public string PlayerName { get; set; }

            [JsonProperty("playerRace")]
            public long PlayerRace { get; set; }

            [JsonProperty("playerClass")]
            public long PlayerClass { get; set; }

            [JsonProperty("playerGender")]
            public long PlayerGender { get; set; }

            [JsonProperty("playerLevel")]
            public long PlayerLevel { get; set; }

            [JsonProperty("guildName")]
            public string GuildName { get; set; }

            [JsonProperty("realmName")]
            public string RealmName { get; set; }

            [JsonProperty("realmId")]
            public long RealmId { get; set; }

            [JsonProperty("totalPlayedTime")]
            public string TotalPlayedTime { get; set; }

            [JsonProperty("online")]
            public long Online { get; set; }

            [JsonProperty("lastLogout")]
            public string LastLogout { get; set; }

            [JsonProperty("money")]
            public long Money { get; set; }

            [JsonProperty("arenaPoints")]
            public long ArenaPoints { get; set; }

            [JsonProperty("totalHonorPoints")]
            public long TotalHonorPoints { get; set; }

            [JsonProperty("totalKills")]
            public long TotalKills { get; set; }

            [JsonProperty("todayKills")]
            public long TodayKills { get; set; }

            [JsonProperty("yesterdayKills")]
            public long YesterdayKills { get; set; }

            [JsonProperty("accBanLogs")]
            public List<PInfoAccBanLog> AccBanLogs { get; set; }

            [JsonProperty("charBanLogs")]
            public List<PInfoCharBanLog> CharBanLogs { get; set; }

            [JsonProperty("muteLogs")]
            public List<PInfoMuteLog> MuteLogs { get; set; }

            [JsonProperty("vpDP")]
            public List<VpDp> VpDp { get; set; }
        }

        public partial class PInfoAccBanLog
        {
            [JsonProperty("accountName")]
            public string AccountName { get; set; }

            [JsonProperty("banDate")]
            public string BanDate { get; set; }

            [JsonProperty("unbanDate")]
            public string UnbanDate { get; set; }

            [JsonProperty("duration")]
            public string Duration { get; set; }

            [JsonProperty("bannedBy")]
            public string BannedBy { get; set; }

            [JsonProperty("banReason")]
            public string BanReason { get; set; }
        }

        public partial class PInfoCharBanLog
        {
            [JsonProperty("player")]
            public string Player { get; set; }

            [JsonProperty("banDate")]
            public string BanDate { get; set; }

            [JsonProperty("unbanDate")]
            public string UnbanDate { get; set; }

            [JsonProperty("duration")]
            public string Duration { get; set; }

            [JsonProperty("bannedBy")]
            public string BannedBy { get; set; }

            [JsonProperty("banReason")]
            public string BanReason { get; set; }
        }

        public partial class PInfoMuteLog
        {
            [JsonProperty("player")]
            public string Player { get; set; }

            [JsonProperty("muteDate")]
            public string MuteDate { get; set; }

            [JsonProperty("muteTime")]
            public string MuteTime { get; set; }

            [JsonProperty("mutedBy")]
            public string MutedBy { get; set; }

            [JsonProperty("muteReason")]
            public string MuteReason { get; set; }
        }

        public partial class VpDp
        {
            [JsonProperty("vp")]
            public long VP { get; set; }

            [JsonProperty("dp")]
            public long DP { get; set; }
        }

        public partial class PlayerInfo
        {
            public static List<PlayerInfo> FromJson(string json) => JsonConvert.DeserializeObject<List<PlayerInfo>>(json, Converter.Settings);
        }

        public static async Task<string> GetPlayerInfoJson(string username, string password, string playername, string realmid)
        {
            return await WebTool.GetStringFromPOST(Config.AppUrl, new Dictionary<string, string>
            {
                { "type", "pinfo" },
                { "user", username },
                { "pass", password },
                { "playername", playername },
                { "realmid", realmid }
            });
        }
        #endregion
        #region UNBAN ACCOUNT
        public static async Task UnbanAccount(string username, string password, string targetAccName)
        {
            await WebTool.SendJsonPOST(Config.AppUrl, new Dictionary<string, string>
            {
                { "type", "unban_account" },
                { "user", username },
                { "pass", password },
                { "targetacc", targetAccName }
            });
        }
        #endregion
        #region SOAP RESPONSE COMMANDS
        public partial class SoapResponse
        {
            [JsonProperty("responseMsg")]
            public string ResponseMsg { get; set; }
        }

        public partial class SoapResponse
        {
            public static SoapResponse FromJson(string json) => JsonConvert.DeserializeObject<SoapResponse>(json, Converter.Settings);
        }

        public static async Task<string> GetSendRawCommandJson(string username, string password, string md5secpa, string command, string realmId)
        {
            return await WebTool.GetStringFromPOST(Config.AppUrl, new Dictionary<string, string>
            {
                { "type", "raw_command" },
                { "user", username },
                { "pass", password },
                { "md5secpa", md5secpa },
                { "command", command },
                { "realmid", realmId },
            });
        }

        public static async Task<string> GetUnbanCharacterJson(string username, string password, string playername, string realmId)
        {
            return await WebTool.GetStringFromPOST(Config.AppUrl, new Dictionary<string, string>
            {
                { "type", "unban_character" },
                { "user", username },
                { "pass", password },
                { "playername", playername },
                { "realmid", realmId },
            });
        }

        public static async Task<string> GetKickPlayerJson(string username, string password, string playername, string realmId)
        {
            return await WebTool.GetStringFromPOST(Config.AppUrl, new Dictionary<string, string>
            {
                { "type", "kick_player" },
                { "user", username },
                { "pass", password },
                { "playername", playername },
                { "realmid", realmId },
            });
        }

        public static async Task<string> GetUnstuckPlayerJson(string username, string password, string playername, string realmId)
        {
            return await WebTool.GetStringFromPOST(Config.AppUrl, new Dictionary<string, string>
            {
                { "type", "unstuck_player" },
                { "user", username },
                { "pass", password },
                { "playername", playername },
                { "realmid", realmId },
            });
        }

        public static async Task<string> GetTicketCloseJson(string username, string password, string ticketid, string realmId)
        {
            return await WebTool.GetStringFromPOST(Config.AppUrl, new Dictionary<string, string>
            {
                { "type", "ticket_close" },
                { "user", username },
                { "pass", password },
                { "ticketid", ticketid },
                { "realmid", realmId },
            });
        }

        public static async Task<string> GetTicketDeleteJson(string username, string password, string ticketid, string realmId)
        {
            return await WebTool.GetStringFromPOST(Config.AppUrl, new Dictionary<string, string>
            {
                { "type", "ticket_delete" },
                { "user", username },
                { "pass", password },
                { "ticketid", ticketid },
                { "realmid", realmId },
            });
        }

        public static async Task<string> GetTicketAssignJson(string username, string password, string ticketid, string gmName, string realmId)
        {
            return await WebTool.GetStringFromPOST(Config.AppUrl, new Dictionary<string, string>
            {
                { "type", "ticket_assign" },
                { "user", username },
                { "pass", password },
                { "ticketid", ticketid },
                { "gmname", gmName },
                { "realmid", realmId },
            });
        }

        public static async Task<string> GetTicketUnassignJson(string username, string password, string ticketid, string realmId)
        {
            return await WebTool.GetStringFromPOST(Config.AppUrl, new Dictionary<string, string>
            {
                { "type", "ticket_unassign" },
                { "user", username },
                { "pass", password },
                { "ticketid", ticketid },
                { "realmid", realmId },
            });
        }

        public static async Task<string> GetTicketReplyJson(string username, string password, string reply, string ticketid, string realmId)
        {
            return await WebTool.GetStringFromPOST(Config.AppUrl, new Dictionary<string, string>
            {
                { "type", "ticket_reply" },
                { "user", username },
                { "pass", password },
                { "reply", reply },
                { "ticketid", ticketid },
                { "realmid", realmId },
            });
        }

        public static async Task<string> GetTicketCompleteJson(string username, string password, string ticketid, string realmId)
        {
            return await WebTool.GetStringFromPOST(Config.AppUrl, new Dictionary<string, string>
            {
                { "type", "ticket_complete" },
                { "user", username },
                { "pass", password },
                { "ticketid", ticketid },
                { "realmid", realmId },
            });
        }

        

        public static async Task<string> GetBanAccountJson(string username, string password, string accountName, string banTime, string banReason, string realmId)
        {
            return await WebTool.GetStringFromPOST(Config.AppUrl, new Dictionary<string, string>
            {
                { "type", "ban_account" },
                { "user", username },
                { "pass", password },
                { "accountname", accountName },
                { "bantime", banTime },
                { "banreason", banReason },
                { "realmid", realmId },
            });
        }

        public static async Task<string> GetBanCharacterJson(string username, string password, string charname, string banTime, string banReason, string realmId)
        {
            return await WebTool.GetStringFromPOST(Config.AppUrl, new Dictionary<string, string>
            {
                { "type", "ban_character" },
                { "user", username },
                { "pass", password },
                { "charname", charname },
                { "bantime", banTime },
                { "banreason", banReason },
                { "realmid", realmId },
            });
        }

        public static async Task<string> GetSetGMLevelJson(string username, string password, string md5secpa, string accountName, string gmLevel, string gmRealmid, string realmId)
        {
            return await WebTool.GetStringFromPOST(Config.AppUrl, new Dictionary<string, string>
            {
                { "type", "set_gmlevel" },
                { "user", username },
                { "pass", password },
                { "md5secpa", md5secpa },
                { "accountname", accountName },
                { "gmlevel", gmLevel },
                { "gmrealmid", gmRealmid },
                { "realmid", realmId },
            });
        }

        public static async Task<string> GetSetAccountPasswordJson(string username, string password, string md5secpa, string accountName, string newPassword, string realmId)
        {
            return await WebTool.GetStringFromPOST(Config.AppUrl, new Dictionary<string, string>
            {
                { "type", "set_password" },
                { "user", username },
                { "pass", password },
                { "md5secpa", md5secpa },
                { "accountname", accountName },
                { "newpassword", newPassword },
                { "realmid", realmId },
            });
        }
        #endregion
        #region REALMS LIST
        public partial class RealmsList
        {
            [JsonProperty("realmId")]
            public long RealmId { get; set; }

            [JsonProperty("realmName")]
            public string RealmName { get; set; }
        }

        public partial class RealmsList
        {
            public static List<RealmsList> FromJson(string json) => JsonConvert.DeserializeObject<List<RealmsList>>(json, Converter.Settings);
        }

        public static async Task<string> GetRealmsListJson(string username, string password)
        {
            return await WebTool.GetStringFromPOST(Config.AppUrl, new Dictionary<string, string>
            {
                { "type", "realms_list" },
                { "user", username },
                { "pass", password }
            });
        }
        #endregion
        #region GM RANKS LIST
        public partial class GMRanksList
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("rank")]
            public long Rank { get; set; }
        }

        public partial class GMRanksList
        {
            public static List<GMRanksList> FromJson(string json) => JsonConvert.DeserializeObject<List<GMRanksList>>(json, Converter.Settings);
        }

        public static async Task<string> GetGMRanksListJson(string username, string password)
        {
            return await WebTool.GetStringFromPOST(Config.AppUrl, new Dictionary<string, string>
            {
                { "type", "gmranks_list" },
                { "user", username },
                { "pass", password }
            });
        }
        #endregion
    }

    /*------------------------------------------------------------------------------------------------
     ------------------------------------ SINS HISTORY CLASS -----------------------------------------
     ------------------------------------------------------------------------------------------------*/
    public class SinsHistoryClass
    {
        #region SINS HISTORY LIST
        public partial class SinsHistoryList
        {
            [JsonProperty("accBanLogs")]
            public List<AccBanLog> AccBanLogs { get; set; }

            [JsonProperty("charBanLogs")]
            public List<CharBanLog> CharBanLogs { get; set; }
        }

        public partial class AccBanLog
        {
            [JsonProperty("banDate")]
            public string BanDate { get; set; }

            [JsonProperty("unbanDate")]
            public string UnbanDate { get; set; }

            [JsonProperty("duration")]
            public string Duration { get; set; }

            [JsonProperty("reason")]
            public string Reason { get; set; }
        }

        public partial class CharBanLog
        {
            [JsonProperty("charName")]
            public string CharName { get; set; }

            [JsonProperty("banDate")]
            public string BanDate { get; set; }

            [JsonProperty("duration")]
            public string Duration { get; set; }

            [JsonProperty("unbanDate")]
            public string UnbanDate { get; set; }

            [JsonProperty("realm")]
            public string Realm { get; set; }

            [JsonProperty("reason")]
            public string Reason { get; set; }
        }

        public partial class SinsHistoryList
        {
            public static List<SinsHistoryList> FromJson(string json) => JsonConvert.DeserializeObject<List<SinsHistoryList>>(json, Converter.Settings);
        }

        public static async Task<string> GetSinsHistoryListJson(string username, string password)
        {
            return await WebTool.GetStringFromPOST(Config.AppUrl, new Dictionary<string, string>
            {
                { "type", "sins_history_list" },
                { "user", username },
                { "pass", password }
            });
        }
        #endregion
    }

    /*------------------------------------------------------------------------------------------------
    ------------------------------------------- FILES LIST -------------------------------------------
    ------------------------------------------------------------------------------------------------*/
    public class FilesListClass
    {
        #region FILES LIST
        public partial class FilesList
        {
            [JsonProperty("filePath")]
            public string FilePath { get; set; }

            [JsonProperty("fileSize")]
            public long FileSize { get; set; }

            [JsonProperty("modifiedTime")]
            public long ModifiedTime { get; set; }
        }

        public partial class FilesList
        {
            public static List<List<FilesList>> FromJson(string json) => JsonConvert.DeserializeObject<List<List<FilesList>>>(json, Converter.Settings);
        }

        public static async Task<string> GetFilesListJson(int _expansionID)
        {
            return await WebTool.GetStringFromPOST(Config.AppUrl, new Dictionary<string, string>
            {
                { "type", "files_list" },
                { "expansion", _expansionID.ToString() }
            });
        }
        #endregion
        #region ORACLE VERSION
        public partial class LVersionResponse
        {
            [JsonProperty("launcher_version")]
            public string Version { get; set; }
        }

        public partial class LVersionResponse
        {
            public static LVersionResponse FromJson(string json) => JsonConvert.DeserializeObject<LVersionResponse>(json, Converter.Settings);
        }

        public static async Task<string> GetLauncherVersionResponseJson()
        {
            return await WebTool.GetStringFromPOST(Config.AppUrl, new Dictionary<string, string>
            {
                { "type", "launcher_version" }
            });
        }
        #endregion
    }

    /*------------------------------------------------------------------------------------------------
    ------------------------------------------- WEB TOOL ---------------------------------------------
    ------------------------------------------------------------------------------------------------*/
    public class WebTool
    {
        public static async Task<string> GetStringFromPOST(string URL, Dictionary<string, string> values)
        {
            try
            {
                var content = new FormUrlEncodedContent(values);

                using (var client = new HttpClient())
                {
                    var response = await client.PostAsync(URL, content);
                    return await response.Content.ReadAsStringAsync();
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        public static async Task SendJsonPOST(string URL, Dictionary<string, string> values)
        {
            try
            {
                var content = new FormUrlEncodedContent(values);

                using (var client = new HttpClient())
                {
                    var response = await client.PostAsync(URL, content);
                }
            }
            catch
            {

            }
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }
    }
}
