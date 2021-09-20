using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class GameOptions
    {
        public static float SWIPE_LIMIT_RECT = 4f;

        public static float SWIPING_SPEED = .05f;

        public static byte NUMBER_OF_ARRAYS;

        public static int DEFAULT_NUMBER_OF_ARRAYS { get; } = 3;

        public static byte MIN_NUMBER_OF_ARRAYS { get; } = 2;

        public static byte MAX_NUMBER_OF_ARRAYS { get; } = 8;

        public static List<ViewResources> SELECTED_RESOURCE;

        public static float BUTTON_SHUFFLE_ANIM_DURATION { get; } = .2f;

        public static int DEFAULT_SMART_SPEED { get; } = 500;

        public static short SMART_SPEED = 1000;

        public static long TIME_FOR_THE_GAME = 10000;

        public static bool VIEWS_SHOW_TEXT = false;

        public static bool VIEWS_SHOW_BITMAP = true;

        public static int DISTANCE_OF_SWIPING = 8;

        public static List<int> unit_of_time { get; } = new List<int> { 10, 45, 115, 250, 410, 720, 1130 };

        public static List<List<int>> COLLECTION_OF_BITMAPS_ID { get; } = new List<List<int>>();

        public static List<List<Color>> COLLECTION_OF_COLORS { get; } = new List<List<Color>>();

        public static List<ViewResources> COLLECTION_OF_SWIPE_VIEW_RESOURCES = new List<ViewResources>();

        public static void init()
        {
            for(int i = 0; i < 10; i++)
            {
                List<Color> colors = new List<Color>();

                for (int j = 0; j < MAX_NUMBER_OF_ARRAYS; j++)
                    colors.Add(UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f));

                COLLECTION_OF_COLORS.Add(colors);
            }

            int r = UnityEngine.Random.Range(0, 10);
            for (int i = 0; i < NUMBER_OF_ARRAYS; i++)
            {
                COLLECTION_OF_SWIPE_VIEW_RESOURCES.Add(new ViewResources()
                {
                    Id = i,
                    Text = i.ToString(),
                    Color = COLLECTION_OF_COLORS[r][i],
                    Image = null
                });

            }
        }
    }
}
