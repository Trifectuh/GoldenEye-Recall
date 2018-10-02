﻿using System;
using System.Threading;
using System.Net.Http;
using Screen;
using System.Text;

namespace GoldenEyeRecall
{
	class Program
	{
		static void Main(string[] args)
		{
			var p = new ScreenReader();
			var playing = false;
			while (ScreenReader.proc == null)
			{
				p.GetObsInstance();
			}

			while (true)
			{
				if (!p.IsLoading())
				{
					PlayVid();
					playing = true;
				}
				else
				{
					if (playing)
					{
						ResetVid();
						playing = false;
					}
				}
				Thread.Sleep(14);
			}
		}

		static async void PlayVid()
		{
			string commandUrl = "http://127.0.0.1:8080/requests/status.xml?command=pl_play";
			var byteArray = Encoding.ASCII.GetBytes(":terryjr");
			using (HttpClient client = new HttpClient())
			{
				client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
				try
				{
					HttpResponseMessage res = await client.GetAsync(commandUrl);
				}
				catch
				{
					Console.WriteLine("Cannot connect to VLC over HTTP");
					Thread.Sleep(10000);
				}
			}
		}

		static async void ResetVid()
		{
			string pauseUrl = "http://127.0.0.1:8080/requests/status.xml?command=pl_pause";
			string resetUrl = "http://127.0.0.1:8080/requests/status.xml?command=seek&val=0";
			var byteArray = Encoding.ASCII.GetBytes(":terryjr");
			using (HttpClient client = new HttpClient())
			{
				client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
				try
				{
					HttpResponseMessage res = await client.GetAsync(pauseUrl);
					HttpResponseMessage res2 = await client.GetAsync(resetUrl);
				}
				catch
				{
					Console.WriteLine("Cannot connect to VLC over HTTP");
					Thread.Sleep(10000);
				}
			}
		}
	}
}
