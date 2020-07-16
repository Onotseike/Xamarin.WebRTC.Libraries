// onotseike@hotmail.comPaula Aliu
using System;
using Android.Media;

namespace WebRTC.Android
{
    public enum AudioDevice
    {
        None,
        SpeakerPhone,
        WiredHeadset,
        Earpiece,
        Bluetooth
    }

    public enum AudioManagerState
    {
        Uninitialized,
        PreInitialized,
        Running,
    }

    public interface IAudioManagerEvents
    {
        void OnAudioDeviceChanged(AudioDevice audioDevice, AudioDevice[] availableAudioDevice);
    }

    //TODO(vali): finish impl 
    //https://webrtc.googlesource.com/src/+/refs/heads/master/examples/androidapp/src/org/appspot/apprtc/AppRTCAudioManager.java
    public class RTCAudioManager
    {
        private const string TAG = nameof(RTCAudioManager);
        private const string SpeakerPhoneAuto = "auto";
        private const string SpeakerPhoneTrue = "true";
        private const string SpeakerPhoneFalse = "false";

        private AudioManager _audioManager;

        private IAudioManagerEvents _audioManagerEvents;
        private AudioManagerState _amState;

        private int _savedAudioMode = 0;

        // Default audio device; speaker phone for video calls or earpiece for audio
        // only calls.
        private AudioDevice _defaultAudioDevice;

        // Contains the currently selected audio device.
        // This device is changed automatically using a certain scheme where e.g.
        // a wired headset "wins" over speaker phone. It is also possible for a
        // user to explicitly select a device (and overrid any predefined scheme).
        // See |userSelectedAudioDevice| for details.
        private AudioDevice _selectedAudioDevice;
        // Contains the user-selected audio device which overrides the predefined
        // selection scheme.
        // explicit selection based on choice by userSelectedAudioDevice.
        private AudioDevice _userSelectedAudioDevice;


    }
}
