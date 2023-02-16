using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using IniParser.Parser;
using IniParser.Model;
using IniParser.Exceptions;
using IniParser;
using System;

namespace TutorialGame.Engine
{
    public sealed class ConfigData
    {
        public GraphicsDevice GraphicsDevice { get; }
        public string ConfigFilePath { get; }

        public GameConsts.ScreenMode ScreenMode { get; set; }
        public Vector2 GameWindowScreenSize { get; set; }
        public Vector2 FullScreenSize { get; set; }
        public bool VSyncActive { get; set; }
        public int MaxRefreshRate { get; set; }
        public GameConsts.WindowPosition GameWindowPosition { get; set; }
        public Vector2 GameWindowPositionCoords { get; set; }

        public ConfigData(GraphicsDevice graphicsDevice, string configFilePath = GameConsts.CONFIG_FILE_PATH)
        {
            GraphicsDevice = graphicsDevice;
            ConfigFilePath = configFilePath;
            ReadConfigData();
        }

        private void ReadConfigData()
        {
            IniData data;

            if (File.Exists(ConfigFilePath))
            {
                var parser = new FileIniDataParser();
                data = parser.ReadFile(ConfigFilePath);
            }
            else
            {
                using (var stream = File.Create(ConfigFilePath)) { }

                var parser = new FileIniDataParser();
                data = parser.ReadFile(ConfigFilePath);

                data[GameConsts.CONFIG_VIDEO_SECTION_NAME][GameConsts.CONFIG_SCREEN_MODE_KEY_NAME] = GameConsts.DEFAULT_SCREEN_MODE.ToString();
                data[GameConsts.CONFIG_VIDEO_SECTION_NAME][GameConsts.CONFIG_GAME_WINDOW_WIDTH_KEY_NAME] = GameConsts.DEFAULT_GAME_WINDOW_WIDTH.ToString();
                data[GameConsts.CONFIG_VIDEO_SECTION_NAME][GameConsts.CONFIG_GAME_WINDOW_HEIGHT_KEY_NAME] = GameConsts.DEFAULT_GAME_WINDOW_HEIGHT.ToString();
                data[GameConsts.CONFIG_VIDEO_SECTION_NAME][GameConsts.CONFIG_FULL_SCREEN_WIDTH_KEY_NAME] = GraphicsDevice.Adapter.CurrentDisplayMode.Width.ToString();
                data[GameConsts.CONFIG_VIDEO_SECTION_NAME][GameConsts.CONFIG_FULL_SCREEN_HEIGHT_KEY_NAME] = GraphicsDevice.Adapter.CurrentDisplayMode.Height.ToString();
                data[GameConsts.CONFIG_VIDEO_SECTION_NAME][GameConsts.CONFIG_VSYNC_ACTIVE_KEY_NAME] = GameConsts.DEFAULT_VSYNC_ACTIVE.ToString();
                data[GameConsts.CONFIG_VIDEO_SECTION_NAME][GameConsts.CONFIG_MAX_REFRESH_RATE_KEY_NAME] = GameConsts.DEFAULT_MAX_REFRESH_RATE.ToString();
                data[GameConsts.CONFIG_VIDEO_SECTION_NAME][GameConsts.CONFIG_GAME_WINDOW_POSITION_KEY_NAME] = GameConsts.DEFAULT_GAME_WINDOW_POSITION.ToString();
                data[GameConsts.CONFIG_VIDEO_SECTION_NAME][GameConsts.CONFIG_GAME_WINDOW_POSITION_COORDS_X_KEY_NAME] = GameConsts.DEFAULT_GAME_WINDOW_POSITION_COORDS_X.ToString();
                data[GameConsts.CONFIG_VIDEO_SECTION_NAME][GameConsts.CONFIG_GAME_WINDOW_POSITION_COORDS_Y_KEY_NAME] = GameConsts.DEFAULT_GAME_WINDOW_POSITION_COORDS_Y.ToString();
                parser.WriteFile(ConfigFilePath, data);
            }

            // Read ScreenMode option property from the configuration file - if the key is not found or the value read cannot be
            // parsed properly then the default value is used as taken from the GameConsts class
            if (data[GameConsts.CONFIG_VIDEO_SECTION_NAME][GameConsts.CONFIG_SCREEN_MODE_KEY_NAME] != null)
            {
                try
                {
                    ScreenMode = GameConsts.ScreenModeEnum.GetEnum(data[GameConsts.CONFIG_VIDEO_SECTION_NAME][GameConsts.CONFIG_SCREEN_MODE_KEY_NAME]);
                }
                catch
                {
                    Console.WriteLine("ScreenMode value not correct type so cannot be properly parsed");
                    ScreenMode = GameConsts.DEFAULT_SCREEN_MODE;
                }
            }
            else
            {
                Console.WriteLine("ScreenMode key not found");
                ScreenMode = GameConsts.DEFAULT_SCREEN_MODE;
            }

            // Read GameWindowScreenSize option property from the configuration file - if the key is not found or the value read
            // cannot be parsed properly then the default values for width and height are used as taken from the GameConsts class
            // in addition if the width and height values do not conform to the accepted allowable screen resolutions then again
            // default values are used as taken from the GameConsts class
            var Width = int.Parse(data[GameConsts.CONFIG_VIDEO_SECTION_NAME][GameConsts.CONFIG_GAME_WINDOW_WIDTH_KEY_NAME]);
            var Height = int.Parse(data[GameConsts.CONFIG_VIDEO_SECTION_NAME][GameConsts.CONFIG_GAME_WINDOW_HEIGHT_KEY_NAME]);
            GameWindowScreenSize = new Vector2(Width, Height);

            // Read FullScreenSize option property from the configuration file - if the key is not found or the value read
            // cannot be parsed properly then the default values for width and height are used as taken from the GameConsts class
            // in addition if the read width and height values do not conform to the accepted allowable screen resolutions then again
            // default values are used as taken from the GameConsts class
            Width = int.Parse(data[GameConsts.CONFIG_VIDEO_SECTION_NAME][GameConsts.CONFIG_FULL_SCREEN_WIDTH_KEY_NAME]);
            Height = int.Parse(data[GameConsts.CONFIG_VIDEO_SECTION_NAME][GameConsts.CONFIG_FULL_SCREEN_HEIGHT_KEY_NAME]);
            FullScreenSize = new Vector2(Width, Height);
            
            // Read VSyncActive option property from the configuration file - if the key is not found or the value read cannot be
            // parsed properly then use the default value taken from the GameConsts class
            if (data[GameConsts.CONFIG_VIDEO_SECTION_NAME][GameConsts.CONFIG_VSYNC_ACTIVE_KEY_NAME] != null)
            {
                try
                {
                    VSyncActive = bool.Parse(data[GameConsts.CONFIG_VIDEO_SECTION_NAME][GameConsts.CONFIG_VSYNC_ACTIVE_KEY_NAME]);
                }
                catch
                {
                    Console.WriteLine("VSyncActive value not correct type so cannot be properly parsed");
                    VSyncActive = GameConsts.DEFAULT_VSYNC_ACTIVE;
                }
            }
            else
            {
                Console.WriteLine("VSyncActive key not found");
                VSyncActive = GameConsts.DEFAULT_VSYNC_ACTIVE;
            }

            // Read MaxRefreshRate option property from the configuration file - if the key is not found or the value read cannot be
            // parsed properly then use the default value taken from the GameConsts class in addition if the read value is below 0
            // then it is assigned as 0 (there is no negative value allowed for this property) - a value of 0 means unlimited 
            // refresh rate
            if (data[GameConsts.CONFIG_VIDEO_SECTION_NAME][GameConsts.CONFIG_MAX_REFRESH_RATE_KEY_NAME] != null)
            {
                try
                {
                    MaxRefreshRate = int.Parse(data[GameConsts.CONFIG_VIDEO_SECTION_NAME][GameConsts.CONFIG_MAX_REFRESH_RATE_KEY_NAME]);
                    MaxRefreshRate = Math.Max(0, MaxRefreshRate);
                }
                catch
                {
                    Console.WriteLine("MaxRefreshRate value not correct type so cannot be properly parsed");
                    MaxRefreshRate = GameConsts.DEFAULT_MAX_REFRESH_RATE;
                }
            }
            else
            {
                Console.WriteLine("MaxRefreshRate key not found");
                MaxRefreshRate = GameConsts.DEFAULT_MAX_REFRESH_RATE;
            }

            // Read GameWindowPosition option property from the configuration file - if the key is not found or the value read cannot be
            // parsed properly then the default value is used as taken from the GameConsts class
            if (data[GameConsts.CONFIG_VIDEO_SECTION_NAME][GameConsts.CONFIG_GAME_WINDOW_POSITION_KEY_NAME] != null)
            {
                try
                {
                    GameWindowPosition = GameConsts.WindowPositionEnum.GetEnum(data[GameConsts.CONFIG_VIDEO_SECTION_NAME]
                                                                                   [GameConsts.CONFIG_GAME_WINDOW_POSITION_KEY_NAME]);
                }
                catch
                {
                    Console.WriteLine("GameWindowPosition value not correct type so cannot be properly parsed");
                    GameWindowPosition = GameConsts.DEFAULT_GAME_WINDOW_POSITION;
                }
            }
            else
            {
                Console.WriteLine("GameWindowPosition key not found");
                GameWindowPosition = GameConsts.DEFAULT_GAME_WINDOW_POSITION;
            }

            // Read GameWindowPositionCoords option property from the configuration file - if the key is not found or the value read
            // cannot be parsed properly then the default value is used as taken from the GameConsts class, note these will only be
            // relevant if the GameWindowPosition option property has the value GameConsts.WindowPositionEnum.UserDefined
            var CoordsX = int.Parse(data[GameConsts.CONFIG_VIDEO_SECTION_NAME][GameConsts.CONFIG_GAME_WINDOW_POSITION_COORDS_X_KEY_NAME]);
            var CoordsY = int.Parse(data[GameConsts.CONFIG_VIDEO_SECTION_NAME][GameConsts.CONFIG_GAME_WINDOW_POSITION_COORDS_Y_KEY_NAME]);
            GameWindowPositionCoords = new Vector2(CoordsX, CoordsY);
        }

        public void WriteConfigData()
        {
            using (var stream = File.Create(ConfigFilePath)) { }

            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile(ConfigFilePath);

            data[GameConsts.CONFIG_VIDEO_SECTION_NAME][GameConsts.CONFIG_SCREEN_MODE_KEY_NAME] = ScreenMode.ToString();
            data[GameConsts.CONFIG_VIDEO_SECTION_NAME][GameConsts.CONFIG_GAME_WINDOW_WIDTH_KEY_NAME] = GameWindowScreenSize.X.ToString();
            data[GameConsts.CONFIG_VIDEO_SECTION_NAME][GameConsts.CONFIG_GAME_WINDOW_HEIGHT_KEY_NAME] = GameWindowScreenSize.Y.ToString();
            data[GameConsts.CONFIG_VIDEO_SECTION_NAME][GameConsts.CONFIG_FULL_SCREEN_WIDTH_KEY_NAME] = FullScreenSize.X.ToString();
            data[GameConsts.CONFIG_VIDEO_SECTION_NAME][GameConsts.CONFIG_FULL_SCREEN_HEIGHT_KEY_NAME] = FullScreenSize.Y.ToString();
            data[GameConsts.CONFIG_VIDEO_SECTION_NAME][GameConsts.CONFIG_VSYNC_ACTIVE_KEY_NAME] = VSyncActive.ToString();
            data[GameConsts.CONFIG_VIDEO_SECTION_NAME][GameConsts.CONFIG_MAX_REFRESH_RATE_KEY_NAME] = MaxRefreshRate.ToString();
            data[GameConsts.CONFIG_VIDEO_SECTION_NAME][GameConsts.CONFIG_GAME_WINDOW_POSITION_KEY_NAME] = GameWindowPosition.ToString();
            data[GameConsts.CONFIG_VIDEO_SECTION_NAME][GameConsts.CONFIG_GAME_WINDOW_POSITION_COORDS_X_KEY_NAME] = GameWindowPositionCoords.X.ToString();
            data[GameConsts.CONFIG_VIDEO_SECTION_NAME][GameConsts.CONFIG_GAME_WINDOW_POSITION_COORDS_Y_KEY_NAME] = GameWindowPositionCoords.Y.ToString();
            parser.WriteFile(ConfigFilePath, data);
        }
    }
}
