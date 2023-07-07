using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HalconDotNet;

namespace HalconAndWPF_co_programming
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private HImage image;
        HTuple hv_ModelID;
        private void btn_LoadImage(object sender, RoutedEventArgs e)
        {
            //创建图像对象
            image = new HImage();
            //读取图像
            image.ReadImage("Images\\board-01.png");
            image.GetImageSize(out int width, out int heigth);
            hSmart.HalconWindow.SetPart(0, 0, heigth, width);
            //设置自适应容器
            hSmart.HalconWindow.DispObj(image);
            hSmart.SetFullImagePart();
        }

        List<HDrawingObject> hDrawingObjects = new List<HDrawingObject>();//生成的ROI区域
        List<DrawingObjectExtension> drawingObjectExtensions = new List<DrawingObjectExtension>(); //ROI拖拽移动后,保存位置信息
        private void btn_DrawCircle(object sender, RoutedEventArgs e)
        {
            var htuples = new HTuple[] { 100, 100, 50 };
            var circle = HDrawingObject.CreateDrawingObject(
                HDrawingObject.HDrawingObjectType.CIRCLE,
               htuples
            );
            drawingObjectExtensions.Add(new DrawingObjectExtension { DrawObj = circle, HTuples = htuples });
            //注册拖拽的回调
            circle.OnDrag(HDrawingObjectCallback);
            //注册图形尺寸变化的回调
            circle.OnResize(HDrawingObjectCallback);
            hDrawingObjects.Add(circle);
            hSmart.HalconWindow.AttachDrawingObjectToWindow(circle);
        }

        private void btn_DrawRectangle(object sender, RoutedEventArgs e)
        {
            var htuples = new HTuple[] { 100, 100, 150, 200 };
            var rectangle = HDrawingObject.CreateDrawingObject(HDrawingObject.HDrawingObjectType.RECTANGLE1,
                htuples);
            hDrawingObjects.Add(rectangle);
            drawingObjectExtensions.Add(new DrawingObjectExtension { DrawObj = rectangle, HTuples = htuples });
            rectangle.OnDrag(HDrawingObjectCallback);
            rectangle.OnResize(HDrawingObjectCallback);
            hSmart.HalconWindow.AttachDrawingObjectToWindow(rectangle);
        }

        private void btn_DrawEllipse(object sender, RoutedEventArgs e)
        {
            var ellipse = HDrawingObject.CreateDrawingObject(HDrawingObject.HDrawingObjectType.ELLIPSE,
           new HTuple[] { 200, 200, 0, 100, 60 });

            hDrawingObjects.Add(ellipse);
            hSmart.HalconWindow.AttachDrawingObjectToWindow(ellipse);
        }
        /// <summary>
        /// 拖拽,改变大小的回调方法
        /// </summary>
        /// <param name="drawid"></param>
        /// <param name="window"></param>
        /// <param name="type"></param>
        private void HDrawingObjectCallback(HDrawingObject drawid, HWindow window, string type)
        {
            RefreshDrawObj(drawid);
        }
        private void RefreshDrawObj(HDrawingObject drawid)
        {
            string drawType = drawid.GetDrawingObjectParams("type");
            if (drawType == "circle")
            {
                //获取当前拖拽对象的坐标和半径          
                var row = drawid.GetDrawingObjectParams("row");
                var column = drawid.GetDrawingObjectParams("column");
                var radius = drawid.GetDrawingObjectParams("radius");
                //封装成一个数组
                var htuples = new HTuple[] { row, column, radius };
                //drawingObjectExtensions
                //获取拖拽对象,判断是否为同一对象
                var obj = drawingObjectExtensions.FirstOrDefault(obj => obj.DrawObj?.ID == drawid.ID);
                if (obj != null)
                {
                    obj.HTuples = htuples;
                }
                Debug.WriteLine($"当前属性值:row:{row}|column:{column}|radius:{radius}");
            }
            else if (drawType == "rectangle1")
            {
                //获取当前拖拽对象的坐标和半径          
                var row1 = drawid.GetDrawingObjectParams("row1");
                var column1 = drawid.GetDrawingObjectParams("column1");
                var row2 = drawid.GetDrawingObjectParams("row2");
                var column2 = drawid.GetDrawingObjectParams("column2");
                //封装成一个数组
                var htuples = new HTuple[] { row1, column1, row2, column2 };
                //drawingObjectExtensions
                //获取拖拽对象,判断是否为同一对象
                var obj = drawingObjectExtensions.FirstOrDefault(obj => obj.DrawObj?.ID == drawid.ID);
                if (obj != null)
                {
                    obj.HTuples = htuples;
                }
                Debug.WriteLine($"当前属性值:row1:{row1}|column1:{column1}|row2:{row2}|column2:{column2}");
            }
        }

        /// <summary>
        /// 创建模板的方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_CreateModel(object sender, RoutedEventArgs e)
        {
            var drawObj = drawingObjectExtensions?[0];
            string drawType = drawObj?.DrawObj.GetDrawingObjectParams("type");
            //var drawObj = drawingObjectExtensions.FirstOrDefault(obj => obj.DrawObj.GetDrawingObjectParams("type") == "circle");
            if (drawType == "circle")
            {
                HOperatorSet.GenCircle(out HObject ho_Circle, drawObj.HTuples[0], drawObj.HTuples[1], drawObj.HTuples[2]);
                HOperatorSet.ReduceDomain(image, ho_Circle, out HObject ho_ImageReduced);
                //创建模板
                HOperatorSet.CreateShapeModel(ho_ImageReduced, "auto", 0, (new HTuple(360)).TupleRad()
                    , "auto", "none", "use_polarity", 30, 10, out hv_ModelID);
                hSmart.HalconWindow.ClearWindow();
                hSmart.HalconWindow.DispObj(ho_ImageReduced);
            }
            else if (drawType == "rectangle1")
            {
                HOperatorSet.GenRectangle1(out HObject ho_Rectangle1, drawObj.HTuples[0], drawObj.HTuples[1], drawObj.HTuples[2], drawObj.HTuples[3]);
                HOperatorSet.ReduceDomain(image, ho_Rectangle1, out HObject ho_ImageReduced);
                //创建模板
                HOperatorSet.CreateShapeModel(ho_ImageReduced, "auto", 0, (new HTuple(360)).TupleRad()
                    , "auto", "none", "use_polarity", 30, 10, out hv_ModelID);

                hSmart.HalconWindow.ClearWindow();
                hSmart.HalconWindow.DispObj(ho_ImageReduced);
            }


        }
        /// <summary>
        /// 显示匹配结果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ShowResult(object sender, RoutedEventArgs e)
        {

            //手动释放ROI对象
            if (hDrawingObjects.Count > 0)
            {
                for (int i = 0; i < hDrawingObjects.Count; i++)
                {
                    hDrawingObjects[i].Dispose();
                }
                hDrawingObjects.Clear();
            }
            hSmart.HalconWindow.ClearWindow();
            image.ReadImage("Images\\board-20.png");
            hSmart.HalconWindow.DispObj(image);
            ModelService modelService = new ModelService();
            modelService.Execute(hSmart.HalconWindow, image, hv_ModelID);
        }
    }
    public class DrawingObjectExtension
    {
        public HDrawingObject? DrawObj { get; set; }
        public HTuple[]? HTuples { get; set; }
    }
}
