using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Speech.Synthesis;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Xiangqi.App.Speaker
{
    public class SystemSpeaker
    {
        public static void Speak(string content)
        {
            Task.Run(() =>
            {
                using (SpeechSynthesizer speech = new SpeechSynthesizer())
                {
                    speech.Rate = int.Parse("-2");//语速  介于-10于10之间
                    var voices = speech.GetInstalledVoices();
                    speech.SelectVoice("Microsoft Huihui Desktop");
                    speech.Speak(content);
                }
            });
        }
    }
}
