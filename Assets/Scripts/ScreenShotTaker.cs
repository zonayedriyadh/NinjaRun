using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RiseupLabs.RoyalSlot
{
    public class ScreenShotTaker : MonoBehaviour
    {
        public int imageIndex = 0;
        //public KeyCode ScTakerKeyCode;

        [ContextMenu("TakeScreenShot")]
        public void TakeScreenShot()
        {
            StartCoroutine("ScreenShot");
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine("ScreenShot");
            }
        }

        public IEnumerator ScreenShot()
        {
            yield return new WaitForEndOfFrame();

            Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
            ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            ss.Apply();

            string fileName = "ScreenShot_" + imageIndex + ".png";
            string filePath = System.IO.Path.Combine(Application.temporaryCachePath, fileName);
            Debug.Log("File path -> " + filePath);
            System.IO.File.WriteAllBytes(filePath, ss.EncodeToPNG());
            imageIndex++;
        }
    }
}
