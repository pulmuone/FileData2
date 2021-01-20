using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileData2
{
    public class MyCustomListAdapter : BaseAdapter<Android.Net.Uri>
    {
        List<Android.Net.Uri> _photoList;

        public MyCustomListAdapter(List<Android.Net.Uri> photoList)
        {
            this._photoList = photoList;
        }

        public override Android.Net.Uri this[int position]
        {
            get
            {
                return _photoList[position];
            }
        }

        public override int Count 
        {
            get
            {
                return _photoList.Count;
            }             
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView;

            if (view == null)
            {
                view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.userRow, parent, false);

                var photo = view.FindViewById<ImageView>(Resource.Id.photoImageView);

                view.Tag = new ViewHolder() { Photo = photo };
            }

            var holder = (ViewHolder)view.Tag;

            if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Q)
            {
                var source = ImageDecoder.CreateSource(parent.Context.ContentResolver, _photoList[position]);
                var bitmap = ImageDecoder.DecodeBitmap(source);
                holder.Photo.SetImageBitmap(bitmap);
            }
            else 
            {
                holder.Photo.SetImageURI(_photoList[position]);
            }            

            return view;
        }
    }
}