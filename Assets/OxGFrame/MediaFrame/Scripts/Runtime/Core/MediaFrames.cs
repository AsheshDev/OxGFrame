
using Cysharp.Threading.Tasks;
using OxGFrame.MediaFrame.AudioFrame;
using OxGFrame.MediaFrame.VideoFrame;
using UnityEngine;
using UnityEngine.Audio;

namespace OxGFrame.MediaFrame
{
    public static class MediaFrames
    {
        public static class AudioFrame
        {
            public static T GetComponent<T>(string assetName) where T : AudioBase
            {
                var components = AudioManager.GetInstance().GetMediaComponents<T>(assetName);
                if (components.Length > 0) return components[0];

                return null;
            }

            public static T[] GetComponents<T>(string assetName) where T : AudioBase
            {
                return AudioManager.GetInstance().GetMediaComponents<T>(assetName);
            }

            #region Mixer
            public static void SetMixerExposedParam(AudioMixer mixer, string expParam, float val)
            {
                AudioManager.GetInstance().SetMixerExposedParam(mixer, expParam, val);
            }

            public static void ClearMixerExposedParam(AudioMixer mixer, string expParam)
            {
                AudioManager.GetInstance().ClearMixerExposedParam(mixer, expParam);
            }

            public static void AutoClearMixerExposedParams(AudioMixer mixer)
            {
                AudioManager.GetInstance().AutoClearMixerExposedParams(mixer);
            }

            public static void AutoRestoreMixerExposedParams(AudioMixer mixer)
            {
                AudioManager.GetInstance().AutoRestoreMixerExposedParams(mixer);
            }

            public static void SetMixerSnapshot(AudioMixer mixer, string snapshotName)
            {
                AudioManager.GetInstance().SetMixerSnapshot(mixer, snapshotName);
            }

            public static void SetMixerTransitionToSnapshot(AudioMixer mixer, AudioMixerSnapshot[] snapshots, float[] weights, float timeToReach = 0.02f)
            {
                AudioManager.GetInstance().SetMixerTransitionToSnapshot(mixer, snapshots, weights, timeToReach);
            }

            public static AudioMixerSnapshot GetMixerSnapshot(AudioMixer mixer, string snapshotName)
            {
                return AudioManager.GetInstance().GetMixerSnapshot(mixer, snapshotName);
            }

            public static AudioMixer GetMixerByName(string mixerName)
            {
                return AudioManager.GetInstance().GetMixerByName(mixerName);
            }
            #endregion

            #region Audio
            public static async UniTask Preload(string assetName)
            {
                await AudioManager.GetInstance().Preload(assetName);
            }

            public static async UniTask Preload(string[] assetNames)
            {
                await AudioManager.GetInstance().Preload(assetNames);
            }

            public static async UniTask<AudioBase[]> Play(string assetName, Transform parent = null, int loops = 0)
            {
                return await AudioManager.GetInstance().Play(assetName, parent, loops);
            }

            public static void ResumeAll()
            {
                AudioManager.GetInstance().ResumeAll();
            }

            public static void Stop(string assetName, bool disableEndEvent = false, bool forceDestroy = false)
            {
                AudioManager.GetInstance().Stop(assetName, disableEndEvent, forceDestroy);
            }

            public static void StopAll(bool disableEndEvent = false, bool forceDestroy = false)
            {
                AudioManager.GetInstance().StopAll(disableEndEvent, forceDestroy);
            }

            public static void Pause(string assetName)
            {
                AudioManager.GetInstance().Pause(assetName);
            }

            public static void PauseAll()
            {
                AudioManager.GetInstance().PauseAll();
            }

            public static void ForceUnload(string assetName)
            {
                AudioManager.GetInstance().ForceUnload(assetName);
            }
            #endregion
        }

        public static class VideoFrame
        {
            public static T GetComponent<T>(string assetName) where T : VideoBase
            {
                var components = VideoManager.GetInstance().GetMediaComponents<T>(assetName);
                if (components.Length > 0) return components[0];

                return null;
            }

            public static T[] GetComponents<T>(string assetName) where T : VideoBase
            {
                return VideoManager.GetInstance().GetMediaComponents<T>(assetName);
            }

            #region Video
            public static async UniTask Preload(string assetName)
            {
                await VideoManager.GetInstance().Preload(assetName);
            }

            public static async UniTask Preload(string[] assetNames)
            {
                await VideoManager.GetInstance().Preload(assetNames);
            }

            public static async UniTask<VideoBase[]> Play(string assetName, Transform parent = null, int loops = 0)
            {
                return await VideoManager.GetInstance().Play(assetName, parent, loops);
            }

            public static void ResumeAll()
            {
                VideoManager.GetInstance().ResumeAll();
            }

            public static void Stop(string assetName, bool disableEndEvent = false, bool forceDestroy = false)
            {
                VideoManager.GetInstance().Stop(assetName, disableEndEvent, forceDestroy);
            }

            public static void StopAll(bool disableEndEvent = false, bool forceDestroy = false)
            {
                VideoManager.GetInstance().StopAll(disableEndEvent, forceDestroy);
            }

            public static void Pause(string assetName)
            {
                VideoManager.GetInstance().Pause(assetName);
            }

            public static void PauseAll()
            {
                VideoManager.GetInstance().PauseAll();
            }

            public static void ForceUnload(string assetName)
            {
                VideoManager.GetInstance().ForceUnload(assetName);
            }
            #endregion
        }
    }
}
