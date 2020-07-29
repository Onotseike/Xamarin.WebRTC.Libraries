// onotseike@hotmail.comPaula Aliu
using System;

using Xamarin.Forms;

namespace WebRTC.XFormsApp.Model
{
    public class CallingView : View
    {
        public static readonly BindableProperty RoomIdProperty = BindableProperty.Create(propertyName: "RoomID", returnType: typeof(string), declaringType: typeof(CallingView), defaultValue: "");



        public string RoomID
        {
            get { return (string)GetValue(RoomIdProperty); }
            set { SetValue(RoomIdProperty, value); }
        }
    }
}
