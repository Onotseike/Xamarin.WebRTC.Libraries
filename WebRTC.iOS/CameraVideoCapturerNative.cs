// onotseike@hotmail.comPaula Aliu
using System;
using System.Diagnostics;
using System.Linq;
using AVFoundation;
using CoreMedia;
using WebRTC.Classes;
using WebRTC.Interfaces;
using WebRTC.iOS.Binding;

namespace WebRTC.iOS
{
    internal class CameraVideoCapturerNative : NativeObjectBase, ICameraVideoCapturer
    {
        private const float FramerateLimit = 30.0f;

        private readonly RTCCameraVideoCapturer _capturer;
        private bool _usingFrontCamera;

        private int _videoWidth;
        private int _videoHeight;
        private int _fps;

        public CameraVideoCapturerNative(RTCCameraVideoCapturer capturer, bool frontCamera) : base(capturer)
        {
            _capturer = capturer;
            _usingFrontCamera = frontCamera;
        }

        public bool IsScreencast => false;

        public void StartCapture()
        {
            StartCapture(1280, 720, (int)FramerateLimit);
        }

        public void StartCapture(int videoWidth, int videoHeight, int fps)
        {
            _fps = fps;
            _videoWidth = videoWidth;
            _videoHeight = videoHeight;

            var position = _usingFrontCamera ? AVCaptureDevicePosition.Back : AVCaptureDevicePosition.Front;
            var device = FindDeviceForPosition(position);
            var format = SelectFormatForDevice(device, videoWidth, videoHeight);

            if (format == null)
            {
                Debug.WriteLine($"CameraVideoCapturerNative -> didn't find a valid format for {device}");
                format = RTCCameraVideoCapturer.SupportedFormatsForDevice(device).FirstOrDefault();
            }

            if (format == null)
            {
                Console.WriteLine("CameraVideoCapturerNative -> no valid formats for device {0}", device);
                return;
            }

            fps = SelectFpsForFormat(format);

            _capturer.StartCaptureWithDevice(device, format, fps);
        }

        public void StopCapture()
        {
            _capturer.StopCapture();
        }

        public void SwitchCamera()
        {
            _usingFrontCamera = !_usingFrontCamera;
            StartCapture(_videoWidth, _videoHeight, _fps);
        }

        private AVCaptureDevice FindDeviceForPosition(AVCaptureDevicePosition position)
        {
            var captureDevices = RTCCameraVideoCapturer.CaptureDevices;
            foreach (var device in captureDevices)
            {
                if (device.Position == position)
                {
                    return device;
                }
            }

            return captureDevices.FirstOrDefault();
        }

        private AVCaptureDeviceFormat SelectFormatForDevice(AVCaptureDevice device, int targetWidth, int targetHeight)
        {
            var formats = RTCCameraVideoCapturer.SupportedFormatsForDevice(device);

            AVCaptureDeviceFormat selectedFormat = null;

            var currentDiff = int.MaxValue;

            foreach (var format in formats)
            {
                if (!(format.FormatDescription is CMVideoFormatDescription videoFormatDescription))
                    continue;
                var dimension = videoFormatDescription.Dimensions;
                var pixelFormat = videoFormatDescription.MediaSubType;
                var diff = Math.Abs(targetWidth - dimension.Width) + Math.Abs(targetHeight - dimension.Height);
                if (diff < currentDiff)
                {
                    selectedFormat = format;
                    currentDiff = diff;
                }
                else if (diff == currentDiff && pixelFormat == _capturer.PreferredOutputPixelFormat)
                {
                    selectedFormat = format;
                }
            }

            return selectedFormat;
        }

        private int SelectFpsForFormat(AVCaptureDeviceFormat format)
        {
            var maxSupportedFramerate = 0d;
            foreach (var fpsRange in format.VideoSupportedFrameRateRanges)
            {
                maxSupportedFramerate = Math.Max(maxSupportedFramerate, fpsRange.MaxFrameRate);
            }

            return (int)Math.Min(maxSupportedFramerate, FramerateLimit);
        }
    }
}
