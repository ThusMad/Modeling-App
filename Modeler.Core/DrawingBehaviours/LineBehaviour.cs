using System.Collections.Generic;
using System.Windows;
using GalaSoft.MvvmLight.Messaging;
using Modeler.Core.Models;
using Modeler.Core.Shapes;
using SharpDX.Mathematics.Interop;

namespace Modeler.Core.DrawingBehaviours
{
    public static class LineBehaviour
    {
        public static void ClickBehaviour(List<Point> clickedPositions, Point currentPoint, ShapeBase tempShape, bool horizontalHeldActive, bool verticalHeldActive, DrawModel drawModel)
        {
            switch (clickedPositions.Count)
            {
                case 0:
                    clickedPositions.Add(currentPoint);

                    tempShape = new Line((int)currentPoint.X, (int)currentPoint.Y, (int)currentPoint.X, (int)currentPoint.Y, new RawColor4(1f, 0f, 0f, 0.4f));
                    drawModel.Shapes.Add(tempShape);
                    Messenger.Default.Send<DrawModel>(drawModel);
                    return;
                case 1:
                {
                    if (!horizontalHeldActive)
                    {
                        if (!verticalHeldActive)
                        {
                            tempShape.Data[1] = new RawVector2((int)currentPoint.X, (int)currentPoint.Y);
                        }
                        else
                        {
                            tempShape.Data[1] = new RawVector2((int)currentPoint.X, tempShape.Data[0].Y);
                        }
                    }
                    else
                    {
                        tempShape.Data[1] = new RawVector2(tempShape.Data[0].X, (int)currentPoint.Y);
                    }
                    tempShape.Color = new RawColor4(0f, 0f, 0f, 1f);
                    tempShape.CalculateOuterBox();
                    clickedPositions.Clear();
                    Messenger.Default.Send<DrawModel>(drawModel);
                    break;
                }
            }
        }
    }
}