using AudioSwitcher.AudioApi;
using AudioSwitcher.AudioApi.CoreAudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPCServer.Controllers
{
    public class AudioManipulator
    {
        private CoreAudioDevice defaultDevice;

        public AudioManipulator()
        {
            defaultDevice = new CoreAudioController().DefaultPlaybackDevice;
        }

        public void SetVolume(int newVolume)
        {
            
            defaultDevice.Volume = newVolume;
        }

        public void SetVolume(float newVolume)
        {
            SetVolume((int)newVolume);
        }
    }
}
