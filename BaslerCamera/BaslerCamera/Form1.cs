using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Basler.Pylon; //引入相机操作软件包

namespace BaslerCamera
{
    public partial class Form1 : Form
    {
        //根据相机SN号创建相机
        Camera camera = new Camera("22796063");

        public Form1()
        {
            InitializeComponent();
            //根据SN号创建相机根据创建相机
            camera = new Camera("22796063");
            //开启相机
            camera.Open();
            //设置图片缓存区大小
            camera.Parameters[PLCameraInstance.MaxNumBuffer].SetValue(10);
            //设置相机触发模式为软件触发
            camera.CameraOpened += Configuration.SoftwareTrigger;
            //关闭相机自动曝光
            camera.Parameters[PLCamera.ExposureAuto].SetValue(PLCamera.ExposureAuto.Off);
            //设置相机曝光时间
            camera.Parameters[PLCamera.ExposureTime].SetValue(5000);
        }

        private void MakePicture_Click(object sender, EventArgs e)
        {
            //设置相机开始采集
            camera.StreamGrabber.Start();
            //软件触发相机采集
            camera.ExecuteSoftwareTrigger();
            //获取相机采集结果
            IGrabResult grabResult = camera.StreamGrabber.RetrieveResult(5000, TimeoutHandling.ThrowException);
            //显示相机采集结果
            ImageWindow.DisplayImage(0, grabResult);
            //采集完成判断相机状态，如果仍在采集则关闭相机采集
            if (camera.StreamGrabber.IsGrabbing)
            {
                camera.StreamGrabber.Stop();
            }
        }
    }
}
