﻿using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Content;
using System.IO;
using Android;
using AndroidX.Core.App;
using Android.Provider;
using Android.Graphics;
using System.Collections.Generic;
using Android.Util;

namespace FileData2
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        ListView listView;

        static string[] PERMISSIONS = {
            Manifest.Permission.ReadExternalStorage,
            Manifest.Permission.AccessMediaLocation
        };

        private ImageView imageview;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            ActivityCompat.RequestPermissions(this, PERMISSIONS, 0);
            
            //imageview = FindViewById<ImageView>(Resource.Id.imageView1);
            listView = FindViewById<ListView>(Resource.Id.listView1);

            var btn1 = FindViewById<Button>(Resource.Id.button1);
            btn1.Click += (object sender, System.EventArgs e) =>
            {
                Intent albumIntent = new Intent(Intent.ActionPick, MediaStore.Images.Media.ExternalContentUri);
                //Intent albumIntent = new Intent(Intent.ActionGetContent);
                //albumIntent.AddCategory(Intent.CategoryOpenable);
                albumIntent.SetType("image/*");
                albumIntent.PutExtra(Intent.ExtraAllowMultiple, true);
                albumIntent.PutExtra(Intent.ExtraMimeTypes, new string[] { "image/*" });
                //StartActivityForResult(albumIntent, 0);
                StartActivityForResult(Intent.CreateChooser(albumIntent, "Select Picture"), 0);
            };

            var btn2 = FindViewById<Button>(Resource.Id.button2);
            btn2.Click += (object sender, System.EventArgs e) =>
            {
                //Intent albumIntent = new Intent(Intent.ActionGetContent);
                Intent albumIntent = new Intent(Intent.ActionOpenDocument);
                //albumIntent.SetType(MediaStore.Images.Media.ContentType);
                //Intent albumIntent = new Intent(Intent.ActionOpenDocumentTree);
                //albumIntent.AddCategory(Intent.CategoryOpenable);
                albumIntent.SetType("image/*");
                albumIntent.PutExtra(Intent.ExtraAllowMultiple, true);
                albumIntent.PutExtra(Intent.ExtraMimeTypes, new string[] { "image/*" });
                StartActivityForResult(albumIntent, 0);
                //StartActivityForResult(Intent.CreateChooser(albumIntent, "Select Picture"), 0);
            };

        }


        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (resultCode == Result.Ok)
            {
                if(data != null)
                {
                    if (data.ClipData != null)
                    {
                        ClipData clipData = data.ClipData;

                        if (clipData == null) return;

                        System.Console.WriteLine(clipData.ItemCount);

                        List<Android.Net.Uri> photoList = new List<Android.Net.Uri>();

                        for (int i = 0; i < clipData.ItemCount; i++)
                        {
                            ClipData.Item item = clipData.GetItemAt(i);

                            var uri = item.Uri;
                            //ImagePrint(uri);
                            photoList.Add(uri);
                        }

                        listView.Adapter = new MyCustomListAdapter(photoList);

                    }
                    else if(data.Data != null)
                    {
                        List<Android.Net.Uri> photoList = new List<Android.Net.Uri>();
                        Android.Net.Uri uri = data.Data;
                        //ImagePrint(uri);
                        photoList.Add(uri);
                        listView.Adapter = new MyCustomListAdapter(photoList);
                    }

                }
            }
        }

        private void ImagePrint(Android.Net.Uri uri)
        {
            if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Q)
            {
                var source = ImageDecoder.CreateSource(this.ContentResolver, uri);
                var bitmap = ImageDecoder.DecodeBitmap(source);
                imageview.SetImageBitmap(bitmap);
            }
            else
            {
                imageview.SetImageURI(uri);

                //var cursor = this.ContentResolver.Query(uri, null, null, null, null);

                //if (cursor != null)
                //{
                //    cursor.MoveToNext();
                //    // 이미지 경로를 가져온다.
                //    var index = cursor.GetColumnIndexOrThrow(MediaStore.Images.Media.InterfaceConsts.Data);
                //    var source = cursor.GetString(index);

                //    //var contentUri = ContentUris.WithAppendedId(MediaStore.Images.Media.ExternalContentUri, id);

                //    // 이미지를 생성한다.
                //    var bitmap = BitmapFactory.DecodeFile(source);
                //    imageview.SetImageBitmap(bitmap);
                //}

            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
       
    }
}