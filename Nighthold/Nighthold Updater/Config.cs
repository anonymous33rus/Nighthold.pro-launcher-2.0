namespace Nighthold_Updater
{
    public class Config // Nighthold UPDATER CONFIG
    {
        private static readonly string LauncherAPIUrl = "https://launcher.nighthold.pro"; // DO NOT ADD "/" AT THE END OF THE URL

        /* -----------------------------------------------------------------------------------------------------------------

            DO NOT TOUCH ANYTHING BELOW !!

            UNLESS YOU KNOW WHAT YOU ARE DOING !!

            I WILL NOT BE RESPONSIBLE IF YOU MESS UP WITH THE PATHS BELOW !!

        ----------------------------------------------------------------------------------------------------------------- */

        public static readonly string
            AppUrl = $"{LauncherAPIUrl}" + "/application/application.php";
    }
}
