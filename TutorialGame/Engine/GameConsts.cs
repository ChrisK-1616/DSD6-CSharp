// Author: Chris Knowles
// Date: Jan 2023
// Copyright: Copperhead Labs, (c)2023
// File: GameConsts.cs
// Version: 1.0.0
// Notes: 

using System;

/*
1280 x 1024 Super-eXtended Graphics Array (SXGA)
1366 x 768 High Definition(HD)
1600 x 900 High Definition Plus (HD+)
1920 x 1080 Full High Definition (FHD)
1920 x 1200 Wide Ultra Extended Graphics Array (WUXGA)
2560 x 1440 Quad High Definition (QHD)
3440 x 1440 Wide Quad High Definition (WQHD)
3840 x 2160 4K or Ultra High Definition (UHD)
*/

namespace TutorialGame.Engine
{
    public sealed class GameConsts
    {
        public enum ScreenMode
        {
            FullScreen,
            BorderlessWindow,
            BorderedWindow
        };

        public static class ScreenModeEnum
        {

            public static ScreenMode GetEnum(string type)
            {
                switch (type)
                {
                    case nameof(ScreenMode.FullScreen): return ScreenMode.FullScreen;
                    case nameof(ScreenMode.BorderlessWindow): return ScreenMode.BorderlessWindow;
                    case nameof(ScreenMode.BorderedWindow): return ScreenMode.BorderedWindow;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(type), type, null);
                };
            }
        }

        public enum WindowPosition
        {
            TopLeft,
            TopCentre,
            TopRight,
            CentreLeft,
            Centre,
            CentreRight,
            BottomLeft,
            BottomCentre,
            BottomRight,
            UserDefined
        }

        public static class WindowPositionEnum
        {

            public static WindowPosition GetEnum(string type)
            {
                switch (type)
                {
                    case nameof(WindowPosition.TopLeft): return WindowPosition.TopLeft;
                    case nameof(WindowPosition.TopCentre): return WindowPosition.TopCentre;
                    case nameof(WindowPosition.TopRight): return WindowPosition.TopRight;
                    case nameof(WindowPosition.CentreLeft): return WindowPosition.CentreLeft;
                    case nameof(WindowPosition.Centre): return WindowPosition.Centre;
                    case nameof(WindowPosition.CentreRight): return WindowPosition.CentreRight;
                    case nameof(WindowPosition.BottomLeft): return WindowPosition.BottomLeft;
                    case nameof(WindowPosition.BottomCentre): return WindowPosition.BottomCentre;
                    case nameof(WindowPosition.BottomRight): return WindowPosition.BottomRight;
                    case nameof(WindowPosition.UserDefined): return WindowPosition.UserDefined;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(type), type, null);
                };
            }
        }

        private GameConsts() { }

        public const string GAME_VERSION = "v0.1.0-alpha.1";
        public const string GAME_NAME = $"Tutorial Game [{GAME_VERSION}]";

        public const string CONTENT_ROOT_DIR = "Content";
        public const string CONTENT_IMAGES_DIR = "Images";
        public const string CONTENT_IMAGES_SPLASH_SCREEN_FILE_NAME = $"splash_screen";
        public const string CONTENT_IMAGES_SPLASH_SCREEN_PATH = $"{CONTENT_IMAGES_DIR}/{CONTENT_IMAGES_SPLASH_SCREEN_FILE_NAME}";
        public const string CONTENT_IMAGES_END_SCREEN_FILE_NAME = $"main_menu";
        public const string CONTENT_IMAGES_END_SCREEN_PATH = $"{CONTENT_IMAGES_DIR}/{CONTENT_IMAGES_END_SCREEN_FILE_NAME}";

        public const string CONFIG_FILE_PATH = "./config.ini";
        public const string CONFIG_VIDEO_SECTION_NAME = "Video";
        public const string CONFIG_SCREEN_MODE_KEY_NAME = "ScreenMode";
        public const string CONFIG_GAME_WINDOW_WIDTH_KEY_NAME = "GameWindowWidth";
        public const string CONFIG_GAME_WINDOW_HEIGHT_KEY_NAME = "GameWindowHeight";
        public const string CONFIG_FULL_SCREEN_WIDTH_KEY_NAME = "FullScreenWidth";
        public const string CONFIG_FULL_SCREEN_HEIGHT_KEY_NAME = "FullScreenHeight";
        public const string CONFIG_VSYNC_ACTIVE_KEY_NAME = "VSyncMode";
        public const string CONFIG_MAX_REFRESH_RATE_KEY_NAME = "MaxRefreshRate";
        public const string CONFIG_GAME_WINDOW_POSITION_KEY_NAME = "GameWindowPosition";
        public const string CONFIG_GAME_WINDOW_POSITION_COORDS_X_KEY_NAME = "GameWindowPositionCoordsX";
        public const string CONFIG_GAME_WINDOW_POSITION_COORDS_Y_KEY_NAME = "GameWindowPositionCoordsY";

        public const ScreenMode DEFAULT_SCREEN_MODE = ScreenMode.BorderedWindow;
        public const int DEFAULT_GAME_WINDOW_WIDTH = 1920;
        public const int DEFAULT_GAME_WINDOW_HEIGHT = 1080;
        public const bool DEFAULT_VSYNC_ACTIVE = false;
        public const int DEFAULT_MAX_REFRESH_RATE = 0;
        public const WindowPosition DEFAULT_GAME_WINDOW_POSITION = WindowPosition.Centre;
        public const int DEFAULT_GAME_WINDOW_POSITION_COORDS_X = 160;
        public const int DEFAULT_GAME_WINDOW_POSITION_COORDS_Y = 90;

        public const string FSM_MAIN_NAME = "MainFSM";
        public const string STATE_SPLASH_SCREEN_NAME = "SplashScreen";
        public const string STATE_MAIN_MENU_NAME = "MainMenu";
        public const string STATE_CLOSING_NAME = "Closing";
    }
}
