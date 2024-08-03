using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Threading.Tasks;
using System.Media;


namespace SuperTanks
{
    public static class MusicManager
    {


        private static SoundPlayer sp = new SoundPlayer();

        public static void update()
        {
            
        }
        
        public static void playBackgroundMusic(string _name)
        {
            sp.SoundLocation = @".\Audio\"+ _name + ".wav";
            sp.PlayLooping();
        }
        public static void stopBackgroundMusic()
        {
            sp.Stop();
        }
    }
}
