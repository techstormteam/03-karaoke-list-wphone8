using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Phone.Speech.Recognition;
using Windows.Phone.Speech.Synthesis;

namespace KaraokeList
{
    public static class Speech
    {
        public static SpeechRecognizer recognizer = Speech.recognizer = new SpeechRecognizer();
        public static SpeechSynthesizer synthesizer = Speech.synthesizer = new SpeechSynthesizer();
        public static SpeechRecognizerUI recognizerUI = Speech.recognizerUI = new SpeechRecognizerUI();
    }
}
