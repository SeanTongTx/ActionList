using SeanLib.Core;
using System;
using System.Collections;

using UnityEngine;

using UnityEngine.UI;

public class ScreenShot : ActionBase
{
    public Texture2D texture;

    public Camera target;

    public RawImage UiTexture;

    public override void Play()
    {
        base.Play();
        if (target)
        {
            GetCapture(target, (t, path) =>
            {
                this.texture = t;
                UiTexture.texture = texture;
                this.End();
            });
        }
        else
        {
            GetCaptureAll(
                (t, path) =>
                {
                    this.texture = t;
                    UiTexture.texture = texture;
                    this.End();
                });
        }
    }

    public static void GetCapture(Camera saveCamera, Action<Texture2D, string> onGet)
    {
        CoroutineCall coroutineCall = ParasiticComponent.parasiteHost.AddComponent<CoroutineCall>();
        Action<Texture2D> onEnd = (texture) =>
        {
            Destroy(coroutineCall);
            string fileName = Application.productName + DateTime.Now.ToString("yyyyMdhhmmss");
            onGet(texture, fileName);
        };
        coroutineCall.StartCoroutine(StartCaptureByCamera(saveCamera, onEnd));
    }
    public static void GetCaptureAll(Action<Texture2D, string> onGet)
    {
        CoroutineCall coroutineCall = ParasiticComponent.parasiteHost.AddComponent<CoroutineCall>();
        Action<Texture2D> onEnd = (texture) =>
        {
            Destroy(coroutineCall);
            string fileName = Application.productName + DateTime.Now.ToString("yyyyMdhhmmss");
            onGet(texture, fileName);
        };
        coroutineCall.StartCoroutine(StartCapture(onEnd));
    }
    public static Texture2D Capture(Camera camera)
    {
        // 创建一个RenderTexture对象  
        Rect rect = new Rect(0, 0, Screen.width, Screen.height);
        // 临时设置相关相机的targetTexture为rt, 并手动渲染相关相机  
        //
        //
        RenderTexture.active = camera.targetTexture;
        camera.Render();
        Texture2D screenShot = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);
        screenShot.ReadPixels(rect, 0, 0);// 注：这个时候，它是从RenderTexture.active中读取像素  
        screenShot.Apply();
        //释放相机，销毁渲染贴图
        camera.targetTexture = null;
        RenderTexture.active = null;
        return screenShot;
    }

    public static IEnumerator StartCapture(Action<Texture2D> onEnd)
    {
        Rect rect = new Rect(0, 0, Screen.width, Screen.height);
        //等待渲染线程结束
        yield return new WaitForEndOfFrame();
        Texture2D screenShot = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);
        screenShot.ReadPixels(rect, 0, 0);
        screenShot.Apply();
        //如果需要可以返回截图
        onEnd(screenShot);
        yield return null;
    }
    public static Texture2D CaptureAll()
    {
        Rect rect = new Rect(0, 0, Screen.width, Screen.height);
        Texture2D screenShot = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);
        screenShot.ReadPixels(rect, 0, 0);
        screenShot.Apply();
        return screenShot;
    }
    private static IEnumerator StartCaptureByCamera(Camera mCamera, Action<Texture2D> onEnd)
    {
        Rect mRect = new Rect(0, 0, Screen.width, Screen.height);
        //等待渲染线程结束
        yield return new WaitForEndOfFrame();

        //初始化RenderTexture
        //   RenderTexture mRender = new RenderTexture((int)mRect.width, (int)mRect.height, 0);
        //设置相机的渲染目标
        // mCamera.targetTexture = mRender;
        //开始渲染
        mCamera.Render();
        //激活渲染贴图读取信息
        RenderTexture.active = mCamera.targetTexture;

        Texture2D mTexture = new Texture2D((int)mRect.width, (int)mRect.height, TextureFormat.RGB24, false);
        //读取屏幕像素信息并存储为纹理数据
        mTexture.ReadPixels(mRect, 0, 0);
        //应用
        mTexture.Apply();

        //释放相机，销毁渲染贴图
        mCamera.targetTexture = null;
        RenderTexture.active = null;
        // Object.Destroy(mRender);
        //如果需要可以返回截图
        onEnd(mTexture);
        yield return null;
    } 
}
