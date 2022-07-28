using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Resources;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Nighthold_Launcher.Nighthold
{
    class ToolHandler
    {
        private enum GenderEnum { MALE, FEMALE, UNKNOWN }

        public static SolidColorBrush GetColorFromHex(string hexColor)
        {
            try
            {
                return new SolidColorBrush(
                    Color.FromArgb(
                        Convert.ToByte(hexColor.Substring(1, 2), 16),
                        Convert.ToByte(hexColor.Substring(3, 2), 16),
                        Convert.ToByte(hexColor.Substring(5, 2), 16),
                        Convert.ToByte(hexColor.Substring(7, 2), 16)
                    )
                );
            }
            catch
            {
                return new SolidColorBrush(Colors.White);
            }
        }

        public static async void CopyButtonTextToClipboard(Button _button)
        {
            var oText = _button.Content.ToString();
            var oForegroundColor = _button.Foreground;

            _button.Foreground = GetColorFromHex("#FF00FF00");
            _button.Content = "Скопировано!";
            AnimHandler.FadeIn(_button, 1000);
            System.Windows.Forms.Clipboard.SetText(oText);

            await Task.Delay(2000);

            _button.Content = oText;
            _button.Foreground = oForegroundColor;
        }

        public static async void CopyTextBlockTextToClipboard(TextBlock _textBlock)
        {
            var oText = _textBlock.Text;
            var oForegroundColor = _textBlock.Foreground;

            _textBlock.Foreground = GetColorFromHex("#FF00FF00");
            _textBlock.Text = "Скопировано!";
            AnimHandler.FadeIn(_textBlock, 1000);
            System.Windows.Forms.Clipboard.SetText(oText);

            await Task.Delay(2000);

            _textBlock.Text = oText;
            _textBlock.Foreground = oForegroundColor;
        }

        public static string StringToMD5(string input)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }

            return hash.ToString();
        }

        public static async Task<bool> ImageExistsAtUrl(string url)
        {
            HttpWebRequest httpReq = (HttpWebRequest)WebRequest.Create(url);
            try
            {
                HttpWebResponse httpRes = (HttpWebResponse)await httpReq.GetResponseAsync();
                if (httpRes.StatusCode == HttpStatusCode.NotFound)
                    return false;

                httpRes.Close();
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static bool RelativeResourceExists(string path)
        {
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                var rm = new ResourceManager(assembly.GetName().Name + ".g", assembly);

                var list = rm.GetResourceSet(CultureInfo.CurrentCulture, true, true);
                foreach (DictionaryEntry item in list)
                {
                    string resName = (string)item.Key;
                    if (resName == path)
                        return true;
                }

                rm.ReleaseAllResources();

            }
            catch (Exception)
            {
                return false;
            }

            return false;
        }

        public static async void SetImageSource(Image _image, string _uri, UriKind _uriKind)
        {
            if (_uri != null)
            {
                if (_uriKind == UriKind.Absolute)
                {
                    if (!Uri.IsWellFormedUriString(_uri, UriKind.Absolute))
                        return;

                    if (!await ImageExistsAtUrl(_uri))
                        return;
                }

                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri(_uri, _uriKind);
                bitmapImage.EndInit();

                _image.Source = bitmapImage;
            }
        }

        public static bool IsAlliance(long _race)
        {
            switch (_race)
            {
                case 1: // human
                    {
                        return true;
                    }
                case 2: // orc
                    {
                        return false;
                    }
                case 3: // dwarf
                    {
                        return true;
                    }
                case 4: // night elf
                    {
                        return true;
                    }
                case 5: // undead
                    {
                        return false;
                    }
                case 6: // tauren
                    {
                        return false;
                    }
                case 7: // gnome
                    {
                        return true;
                    }
                case 8: // troll
                    {
                        return false;
                    }
                case 9: // goblin
                    {
                        return false;
                    }
                case 10: // blood elf
                    {
                        return false;
                    }
                case 11: // draenei
                    {
                        return true;
                    }
                case 12: // worgen
                    {
                        return true;
                    }
                case 14: // pandaren
                    {
                        return true;
                    }
                case 15: // nightelf_dh
                    {
                        return true;
                    }
                case 16: // bloodelf_dh
                    {
                        return false;
                    }
                case 17: // nightborne
                    {
                        return false;
                    }
                case 18: // vuidelf
                    {
                        return true;
                    }
                case 20: // vulpera
                    {
                        return true;
                    }

                case 22: // worgen
                    {
                        return true;
                    }
                //case 23: // gilnean

                //    break;
                case 24: // pandaren
                case 25: // pandaren
                case 26: // pandaren
                    {
                        return false;
                    }
                case 27: // nightborne
                    {
                        return false;
                    }
                case 28: // highmountain tauren
                    {
                        return false;
                    }
                case 29: // void elf
                    {
                        return true;
                    }
                case 30: // lightforged draenei
                    {
                        return false;
                    }
                case 31: // zandalari troll
                    {
                        return false;
                    }
                case 32: // kul tiran
                    {
                        return true;
                    }
                //case 33: // human

                //    break;
                case 34: // dark iron dwarf
                    {
                        return true;
                    }
                case 35: // vulpera
                    {
                        return false;
                    }
                case 36: // mag'har orc
                    {
                        return false;
                    }
                case 37: // mechagnome
                    {
                        return true;
                    }
                default:
                    break;
            }

            return false;
        }

        public static bool IsHorde(long _race)
        {
            return !IsAlliance(_race);
        }

        public static void SetRaceGenderImage(Image _image, long _race, long _gender)
        {
            switch (_race)
            {
                case 1: // human
                    {
                        if (_gender == (int)GenderEnum.MALE)
                            SetImageSource(_image, $"/{ Assembly.GetCallingAssembly().GetName().Name };component/Assets/Race Icons/Charactercreate-races_human-male.png", UriKind.Relative);
                        else
                            SetImageSource(_image, $"/{ Assembly.GetCallingAssembly().GetName().Name };component/Assets/Race Icons/Charactercreate-races_human-female.png", UriKind.Relative);
                        break;
                    }
                case 2: // orc
                    {
                        if (_gender == (int)GenderEnum.MALE)
                            SetImageSource(_image, $"/{ Assembly.GetCallingAssembly().GetName().Name };component/Assets/Race Icons/Charactercreate-races_orc-male.png", UriKind.Relative);
                        else
                            SetImageSource(_image, $"/{ Assembly.GetCallingAssembly().GetName().Name };component/Assets/Race Icons/Charactercreate-races_orc-female.png", UriKind.Relative);
                        break;
                    }
                case 3: // dwarf
                    {
                        if (_gender == (int)GenderEnum.MALE)
                            SetImageSource(_image, $"/{ Assembly.GetCallingAssembly().GetName().Name };component/Assets/Race Icons/Charactercreate-races_dwarf-male.png", UriKind.Relative);
                        else
                            SetImageSource(_image, $"/{ Assembly.GetCallingAssembly().GetName().Name };component/Assets/Race Icons/Charactercreate-races_dwarf-female.png", UriKind.Relative);
                        break;
                    }
                case 15:
                case 4: // night elf
                    {
                        if (_gender == (int)GenderEnum.MALE)
                            SetImageSource(_image, $"/{ Assembly.GetCallingAssembly().GetName().Name };component/Assets/Race Icons/Charactercreate-races_nightelf-male.png", UriKind.Relative);
                        else
                            SetImageSource(_image, $"/{ Assembly.GetCallingAssembly().GetName().Name };component/Assets/Race Icons/Charactercreate-races_nightelf-female.png", UriKind.Relative);
                        break;
                    }
                case 5: // undead
                    {
                        if (_gender == (int)GenderEnum.MALE)
                            SetImageSource(_image, $"/{ Assembly.GetCallingAssembly().GetName().Name };component/Assets/Race Icons/Charactercreate-races_undead-male.png", UriKind.Relative);
                        else
                            SetImageSource(_image, $"/{ Assembly.GetCallingAssembly().GetName().Name };component/Assets/Race Icons/Charactercreate-races_undead-female.png", UriKind.Relative);
                        break;
                    }
                case 6: // tauren
                    {
                        if (_gender == (int)GenderEnum.MALE)
                            SetImageSource(_image, $"/{ Assembly.GetCallingAssembly().GetName().Name };component/Assets/Race Icons/Charactercreate-races_tauren-male.png", UriKind.Relative);
                        else
                            SetImageSource(_image, $"/{ Assembly.GetCallingAssembly().GetName().Name };component/Assets/Race Icons/Charactercreate-races_tauren-female.png", UriKind.Relative);
                        break;
                    }
                case 7: // gnome
                    {
                        if (_gender == (int)GenderEnum.MALE)
                            SetImageSource(_image, $"/{ Assembly.GetCallingAssembly().GetName().Name };component/Assets/Race Icons/Charactercreate-races_gnome-male.png", UriKind.Relative);
                        else
                            SetImageSource(_image, $"/{ Assembly.GetCallingAssembly().GetName().Name };component/Assets/Race Icons/Charactercreate-races_gnome-female.png", UriKind.Relative);
                        break;
                    }
                case 8: // troll
                    {
                        if (_gender == (int)GenderEnum.MALE)
                            SetImageSource(_image, $"/{ Assembly.GetCallingAssembly().GetName().Name };component/Assets/Race Icons/Charactercreate-races_troll-male.png", UriKind.Relative);
                        else
                            SetImageSource(_image, $"/{ Assembly.GetCallingAssembly().GetName().Name };component/Assets/Race Icons/Charactercreate-races_troll-female.png", UriKind.Relative);
                        break;
                    }
                case 9: // goblin
                    {
                        if (_gender == (int)GenderEnum.MALE)
                            SetImageSource(_image, $"/{ Assembly.GetCallingAssembly().GetName().Name };component/Assets/Race Icons/Charactercreate-races_goblin-male.png", UriKind.Relative);
                        else
                            SetImageSource(_image, $"/{ Assembly.GetCallingAssembly().GetName().Name };component/Assets/Race Icons/Charactercreate-races_goblin-female.png", UriKind.Relative);
                        break;
                    }
                case 16:
                case 10: // blood elf
                    {
                        if (_gender == (int)GenderEnum.MALE)
                            SetImageSource(_image, $"/{ Assembly.GetCallingAssembly().GetName().Name };component/Assets/Race Icons/Charactercreate-races_bloodelf-male.png", UriKind.Relative);
                        else
                            SetImageSource(_image, $"/{ Assembly.GetCallingAssembly().GetName().Name };component/Assets/Race Icons/Charactercreate-races_bloodelf-female.png", UriKind.Relative);
                        break;
                    }
                case 11: // draenei
                    {
                        if (_gender == (int)GenderEnum.MALE)
                            SetImageSource(_image, $"/{ Assembly.GetCallingAssembly().GetName().Name };component/Assets/Race Icons/Charactercreate-races_draenei-male.png", UriKind.Relative);
                        else
                            SetImageSource(_image, $"/{ Assembly.GetCallingAssembly().GetName().Name };component/Assets/Race Icons/Charactercreate-races_draenei-female.png", UriKind.Relative);
                        break;
                    }
                case 12:
                case 22: // worgen
                    {
                        if (_gender == (int)GenderEnum.MALE)
                            SetImageSource(_image, $"/{ Assembly.GetCallingAssembly().GetName().Name };component/Assets/Race Icons/Charactercreate-races_worgen-male2.png", UriKind.Relative);
                        else
                            SetImageSource(_image, $"/{ Assembly.GetCallingAssembly().GetName().Name };component/Assets/Race Icons/Charactercreate-races_worgen-female2.png", UriKind.Relative);
                        break;
                    }
                //case 23: // gilnean

                //    break;
                case 14:
                case 24: // pandaren
                case 25: // pandaren
                case 26: // pandaren
                    {
                        if (_gender == (int)GenderEnum.MALE)
                            SetImageSource(_image, $"/{ Assembly.GetCallingAssembly().GetName().Name };component/Assets/Race Icons/Charactercreate-races_panda-male.png", UriKind.Relative);
                        else
                            SetImageSource(_image, $"/{ Assembly.GetCallingAssembly().GetName().Name };component/Assets/Race Icons/Charactercreate-races_panda-female.png", UriKind.Relative);
                        break;
                    }
                case 17:
                case 27: // nightborne
                    {
                        if (_gender == (int)GenderEnum.MALE)
                            SetImageSource(_image, $"/{ Assembly.GetCallingAssembly().GetName().Name };component/Assets/Race Icons/Charactercreate-races_nightborne-male.png", UriKind.Relative);
                        else
                            SetImageSource(_image, $"/{ Assembly.GetCallingAssembly().GetName().Name };component/Assets/Race Icons/Charactercreate-races_nightborne-female.png", UriKind.Relative);
                        break;
                    }
                case 28: // highmountain tauren
                    {
                        if (_gender == (int)GenderEnum.MALE)
                            SetImageSource(_image, $"/{ Assembly.GetCallingAssembly().GetName().Name };component/Assets/Race Icons/Charactercreate-races_highmountain-male.png", UriKind.Relative);
                        else
                            SetImageSource(_image, $"/{ Assembly.GetCallingAssembly().GetName().Name };component/Assets/Race Icons/Charactercreate-races_highmountain-female.png", UriKind.Relative);
                        break;
                    }
                case 18:
                case 29: // void elf
                    {
                        if (_gender == (int)GenderEnum.MALE)
                            SetImageSource(_image, $"/{ Assembly.GetCallingAssembly().GetName().Name };component/Assets/Race Icons/Charactercreate-races_voidelf-male.png", UriKind.Relative);
                        else
                            SetImageSource(_image, $"/{ Assembly.GetCallingAssembly().GetName().Name };component/Assets/Race Icons/Charactercreate-races_voidelf-female.png", UriKind.Relative);
                        break;
                    }
                case 30: // lightforged draenei
                    {
                        if (_gender == (int)GenderEnum.MALE)
                            SetImageSource(_image, $"/{ Assembly.GetCallingAssembly().GetName().Name };component/Assets/Race Icons/Charactercreate-races_lightforged-male.png", UriKind.Relative);
                        else
                            SetImageSource(_image, $"/{ Assembly.GetCallingAssembly().GetName().Name };component/Assets/Race Icons/Charactercreate-races_lightforged-female.png", UriKind.Relative);
                        break;
                    }
                case 31: // zandalari troll
                    {
                        if (_gender == (int)GenderEnum.MALE)
                            SetImageSource(_image, $"/{ Assembly.GetCallingAssembly().GetName().Name };component/Assets/Race Icons/Charactercreate-races_ZandalariTroll-male.png", UriKind.Relative);
                        else
                            SetImageSource(_image, $"/{ Assembly.GetCallingAssembly().GetName().Name };component/Assets/Race Icons/Charactercreate-races_ZandalariTroll-female.png", UriKind.Relative);
                        break;
                    }
                case 32: // kul tiran
                    {
                        if (_gender == (int)GenderEnum.MALE)
                            SetImageSource(_image, $"/{ Assembly.GetCallingAssembly().GetName().Name };component/Assets/Race Icons/Charactercreate-races_kultiranhuman-male.png", UriKind.Relative);
                        else
                            SetImageSource(_image, $"/{ Assembly.GetCallingAssembly().GetName().Name };component/Assets/Race Icons/Charactercreate-races_kultiranhuman-female.png", UriKind.Relative);
                        break;
                    }
                //case 33: // human

                //    break;
                case 34: // dark iron dwarf
                    {
                        if (_gender == (int)GenderEnum.MALE)
                            SetImageSource(_image, $"/{ Assembly.GetCallingAssembly().GetName().Name };component/Assets/Race Icons/Charactercreate-races_darkirondwarf-male.png", UriKind.Relative);
                        else
                            SetImageSource(_image, $"/{ Assembly.GetCallingAssembly().GetName().Name };component/Assets/Race Icons/Charactercreate-races_darkirondwarf-female.png", UriKind.Relative);
                        break;
                    }
                case 20:
                case 35: // vulpera
                    {
                        if (_gender == (int)GenderEnum.MALE)
                            SetImageSource(_image, $"/{ Assembly.GetCallingAssembly().GetName().Name };component/Assets/Race Icons/CharacterCreate-Races_vulpera-male.png", UriKind.Relative);
                        else
                            SetImageSource(_image, $"/{ Assembly.GetCallingAssembly().GetName().Name };component/Assets/Race Icons/CharacterCreate-Races_vulpera-female.png", UriKind.Relative);
                        break;
                    }
                case 36: // mag'har orc
                    {
                        if (_gender == (int)GenderEnum.MALE)
                            SetImageSource(_image, $"/{ Assembly.GetCallingAssembly().GetName().Name };component/Assets/Race Icons/Charactercreate-races_maghar-male.png", UriKind.Relative);
                        else
                            SetImageSource(_image, $"/{ Assembly.GetCallingAssembly().GetName().Name };component/Assets/Race Icons/Charactercreate-races_maghar-female.png", UriKind.Relative);
                        break;
                    }
                case 37: // mechagnome
                    {
                        if (_gender == (int)GenderEnum.MALE)
                            SetImageSource(_image, $"/{ Assembly.GetCallingAssembly().GetName().Name };component/Assets/Race Icons/Charactercreate-races_mechagnome-male.png", UriKind.Relative);
                        else
                            SetImageSource(_image, $"/{ Assembly.GetCallingAssembly().GetName().Name };component/Assets/Race Icons/Charactercreate-races_mechagnome-female.png", UriKind.Relative);
                        break;
                    }
                default:
                    break;
            }
        }

        public static void SetClassImage(Image _image, long _class)
        {
            switch (_class)
            {
                case 1: // warrior
                    {
                        SetImageSource(_image, $"/{ Assembly.GetCallingAssembly().GetName().Name };component/Assets/Class Icons/Charactercreate-class_warrior.png", UriKind.Relative);
                        break;
                    }
                case 2: // paladin
                    {
                        SetImageSource(_image, $"/{ Assembly.GetCallingAssembly().GetName().Name };component/Assets/Class Icons/Charactercreate-class_paladin.png", UriKind.Relative);
                        break;
                    }
                case 3: // hunter
                    {
                        SetImageSource(_image, $"/{ Assembly.GetCallingAssembly().GetName().Name };component/Assets/Class Icons/Charactercreate-class_hunter.png", UriKind.Relative);
                        break;
                    }
                case 4: // rogue
                    {
                        SetImageSource(_image, $"/{ Assembly.GetCallingAssembly().GetName().Name };component/Assets/Class Icons/Charactercreate-class_rogue.png", UriKind.Relative);
                        break;
                    }
                case 5: // priest
                    {
                        SetImageSource(_image, $"/{ Assembly.GetCallingAssembly().GetName().Name };component/Assets/Class Icons/Charactercreate-class_priest.png", UriKind.Relative);
                        break;
                    }
                case 6: // death knight
                    {
                        SetImageSource(_image, $"/{ Assembly.GetCallingAssembly().GetName().Name };component/Assets/Class Icons/Charactercreate-class_deathknight.png", UriKind.Relative);
                        break;
                    }
                case 7: // shaman
                    {
                        SetImageSource(_image, $"/{ Assembly.GetCallingAssembly().GetName().Name };component/Assets/Class Icons/Charactercreate-class_shaman.png", UriKind.Relative);
                        break;
                    }
                case 8: // mage
                    {
                        SetImageSource(_image, $"/{ Assembly.GetCallingAssembly().GetName().Name };component/Assets/Class Icons/Charactercreate-class_mage.png", UriKind.Relative);
                        break;
                    }
                case 9: // warlock
                    {
                        SetImageSource(_image, $"/{ Assembly.GetCallingAssembly().GetName().Name };component/Assets/Class Icons/Charactercreate-class_warlock.png", UriKind.Relative);
                        break;
                    }
                case 10: // monk
                    {
                        SetImageSource(_image, $"/{ Assembly.GetCallingAssembly().GetName().Name };component/Assets/Class Icons/Charactercreate-class_monk.png", UriKind.Relative);
                        break;
                    }
                case 11: // druid
                    {
                        SetImageSource(_image, $"/{ Assembly.GetCallingAssembly().GetName().Name };component/Assets/Class Icons/Charactercreate-class_druid.png", UriKind.Relative);
                        break;
                    }
                case 12: // demon hunter
                    {
                        SetImageSource(_image, $"/{ Assembly.GetCallingAssembly().GetName().Name };component/Assets/Class Icons/Charactercreate-class_demonhunter.png", UriKind.Relative);
                        break;
                    }

                default:
                    break;
            }
        }

        public static SolidColorBrush GetPlayerClassColorBrush(long _class)
        {
            switch (_class)
            {
                case 1: // warrior
                    {
                        return GetColorFromHex("#FF" + "C69B6D");
                    }
                case 2: // paladin
                    {
                        return GetColorFromHex("#FF" + "F48CBA");
                    }
                case 3: // hunter
                    {
                        return GetColorFromHex("#FF" + "AAD372");
                    }
                case 4: // rogue
                    {
                        return GetColorFromHex("#FF" + "FFF468");
                    }
                case 5: // priest
                    {
                        return GetColorFromHex("#FF" + "FFFFFF");
                    }
                case 6: // death knight
                    {
                        return GetColorFromHex("#FF" + "C41E3A");
                    }
                case 7: // shaman
                    {
                        return GetColorFromHex("#FF" + "0070DD");
                    }
                case 8: // mage
                    {
                        return GetColorFromHex("#FF" + "3FC7EB");
                    }
                case 9: // warlock
                    {
                        return GetColorFromHex("#FF" + "8788EE");
                    }
                case 10: // monk
                    {
                        return GetColorFromHex("#FF" + "00FF98");
                    }
                case 11: // druid
                    {
                        return GetColorFromHex("#FF" + "FF7C0A");
                    }
                case 12: // demon hunter
                    {
                        return GetColorFromHex("#FF" + "A330C9");
                    }

                default:
                    return GetColorFromHex("#FF" + "FFFFFF"); ;
            }
        }

        public static string RaceToName(long _race)
        {
            switch (_race)
            {
                case 1:
                    return "Человек";
                case 2:
                    return "Орк";
                case 3:
                    return "Дворф";
                case 4:
                    return "Ночной эльф";
                case 5:
                    return "Нежить";
                case 6:
                    return "Таурен";
                case 7:
                    return "Гном";
                case 8:
                    return "Троль";
                case 9:
                    return "Гоблин";
                case 10:
                    return "Эльф крови";
                case 11: 
                    return "Дреней";
                case 12:
                    return "Ворген";
                //case 13: // naga
                case 14:
                    return "Пандарен";
                case 15: 
                case 16:
                    return "Охотник на демонов";
                case 17:
                    return "Ночнорожденный";
                case 18:
                    return "Эльф бездны";
                //case 19: // taunka
                case 20:
                    return "Вульпера";
                //case 21: // ice troll
                case 22:
                    return "Worgen";
                //case 23: // gilnean
                case 24: // pandaren
                case 25: // pandaren
                case 26: // pandaren
                    return "Pandaren";
                case 27:
                    return "Nightborne";
                case 28:
                    return "Highmountain Tauren";
                case 29:
                    return "Void Elf";
                case 30:
                    return "Lightforged Draenei";
                case 31:
                    return "Zandalari Troll";
                case 32:
                    return "Kul Tiran";
                //case 33: // human
                case 34:
                    return "Dark Iron Dwarf";
                case 35:
                    return "Vulpera";
                case 36:
                    return "Mag'har Orc";
                case 37:
                    return "Mechagnome";
                default:
                    return "Unknown";
            }
        }

        public static string ClassToName(long _class)
        {
            switch (_class)
            {
                case 1:
                    return "Воин";
                case 2:
                    return "Паладин";
                case 3:
                    return "Охотник";
                case 4:
                    return "Разбойник";
                case 5:
                    return "Жрец";
                case 6:
                    return "Рыцарь Смерти";
                case 7:
                    return "Шаман";
                case 8:
                    return "Маг";
                case 9:
                    return "Чернокнижник";
                case 10:
                    return "Monk";
                case 11:
                    return "Друид";
                case 12:
                    return "Demon Hunter";
                default:
                    return "Unknown";
            }
        }

        public static string ExpansionIdToName(long _expansionId)
        {
            switch (_expansionId)
            {
                case 1:
                    return "Classic";
                case 2:
                    return "The Burning Crusade";
                case 3:
                    return "Wrath of the lich King";
                case 4:
                    return "Cataclysm";
                case 5:
                    return "Mists of Pandaria";
                case 6:
                    return "Warlords of Draenor";
                case 7:
                    return "Legion";
                case 8:
                    return "Battle for Azeroth";
                case 9:
                    return "Shadowlands";
                default:
                    return "Unknown";
            }
        }

        public static string StringBeautify(string str)
        {
            string new_string;

            // Remove empty lines
            new_string = Regex.Replace(str, @"^\s+$[\r\n]*", string.Empty, RegexOptions.Multiline);

            // Remove empty spaces
            new_string = Regex.Replace(new_string, " {2,}", " ");

            // Remove begining and ending of string space
            new_string = new_string.Trim();

            // remove new lines
            new_string = Regex.Replace(new_string, @"\t|\n|\r", "");

            return new_string;
        }

        public static async Task<bool> CheckForInternetConnectionAsync()
        {
            try
            {
                using (var client = new WebClient())
                using (await client.OpenReadTaskAsync(new Uri("http://google.com/generate_204", UriKind.Absolute)))
                    return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsFileDifferent(long _rFileSize, long _rModifiedTime, string _localFilePath)
        {
            try
            {
                FileInfo file = new FileInfo(_localFilePath);

                if (file.Length != _rFileSize)
                    return true;

                long localModifiedTime = ((DateTimeOffset)file.LastWriteTime).ToUnixTimeSeconds();

                if (localModifiedTime != _rModifiedTime)
                    return true;
            }
            catch
            {
                return true;
            }

            return false;
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        public static void FixWowMFILAttributes(string filePath)
        {
            File.SetAttributes(filePath, FileAttributes.ReadOnly);
        }
    }
}
