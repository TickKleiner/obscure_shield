using System;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Globalization;

namespace obscure_shield
{
	public class Parser : ParserUtils
	{
		const string lolz_prefix = "https://lolz.guru/market/vkontakte/?user_id=&category_id=2&pmin=&pmax=&title=https%3A%2F%2Fvk.com%2Fid";
		const string lolz_postfix = "&_itemCount=19531&sex=&min_age=&max_age=&vk_vote_min=&vk_vote_max=&login=&vk_friend_min=&vk_friend_max=&vk_follower_min=&vk_follower_max=&groups_min=&groups_max=&reg=&reg_period=year&tfa=&tel=&email=&group_follower_min=&group_follower_max=&admin_level=&dig_min=&dig_max=&order_by=&_formSubmitted=true&countItemsOnly=true&_xfRequestUri=%2Fmarket%2Fvkontakte%2F%3Freg_period%3Dyear&_xfNoRedirect=1&_xfToken=176776%2C1624754426%2C051c5cf43a33b3e92a3078b464b4e9966c15bb74&_xfResponseType=json";
		const string vk_prefix = "https://vk.com/id";

		string cur_page;

		public Parser() {}

		private void get_page(long _id)
		{
			string url = lolz_prefix + _id.ToString() + lolz_postfix;

			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

			Cookie cookie_0 = new Cookie()
			{
				Name = "df_id",
				Value = "20e471b44b60a985990182f4311727ae",
				Domain = ".lolz.guru",
				Path = "/"
			};
			//string exp = "04.07.2021 04:14:18 +3";
			//cookie_0.Expires = DateTime.ParseExact(exp, "dd.MM.yyyy HH:mm:ss z", CultureInfo.InvariantCulture);

			Cookie cookie_1 = new Cookie()
			{
				Name = "xf_market_search_url",
				Value = "%2Fmarket%2Fvkontakte%2F%3Fuser_id%3D%26category_id%3D2%26pmin%3D%26pmax%3D%26title%3Dhttps%253A%252F%252Fvk.com%252Fid45665356%26_itemCount%3D19531%26sex%3D%26min_age%3D%26max_age%3D%26vk_vote_min%3D%26vk_vote_max%3D%26login%3D%26vk_friend_min%3D%26vk_friend_max%3D%26vk_follower_min%3D%26vk_follower_max%3D%26groups_min%3D%26groups_max%3D%26reg%3D%26reg_period%3Dyear%26tfa%3D%26tel%3D%26email%3D%26group_follower_min%3D%26group_follower_max%3D%26admin_level%3D%26dig_min%3D%26dig_max%3D%26order_by%3D%26_formSubmitted%3Dtrue%26countItemsOnly%3Dtrue%26_xfRequestUri%3D%252Fmarket%252Fvkontakte%252F%253Freg_period%253Dyear%26_xfNoRedirect%3D1%26_xfToken%3D176776%252C1624754426%252C051c5cf43a33b3e92a3078b464b4e9966c15bb74%26_xfResponseType%3Djson",
				Domain = "lolz.guru",
				Path = "/",
				Secure = true
				
			};
			//exp = "27.06.2021 05:48:41 +3";
			//cookie_1.Expires = DateTime.ParseExact(exp, "dd.MM.yyyy HH:mm:ss z", CultureInfo.InvariantCulture);

			Cookie cookie_2 = new Cookie()
			{
				Name = "xf_session",
				Value = "cbaa3fc8cbe78e2d31d441cc0a002d6a",
				Domain = ".lolz.guru",
				Path = "/",
				Secure = true,
				HttpOnly = true
			};

			request.CookieContainer = new CookieContainer();
			request.CookieContainer.Add(new Uri(url), cookie_0);
			request.CookieContainer.Add(new Uri(url), cookie_1);
			request.CookieContainer.Add(new Uri(url), cookie_2);

			request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.114 Safari/537.36";
			request.Method = "GET";

			HttpWebResponse response = (HttpWebResponse)request.GetResponse();

			if (response.StatusCode == HttpStatusCode.OK)
			{
				Stream receiveStream = response.GetResponseStream();
				StreamReader readStream = null;

				if (String.IsNullOrWhiteSpace(response.CharacterSet))
					readStream = new StreamReader(receiveStream);
				else
					readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));

				cur_page = readStream.ReadToEnd();

				response.Close();
				readStream.Close();
			}

		}

		public void parse_id_s(List<long> _id_s)
		{
			uint count = 0;
			uint in_danger_count = 0;
			uint done_count= 0;

			create_res_file();

			foreach (var id in _id_s)
			{
				get_page(id);
				count = get_acc_count(cur_page);

				++done_count;
				if (count != 0)
				{
					++in_danger_count;
					add_str_to_log(vk_prefix + id.ToString(), count);

					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine(vk_prefix + id.ToString() + " in DANGER");
					Console.ForegroundColor = ConsoleColor.White;
				}
				else
				{
					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine(done_count.ToString() + " CHECKED");
					Console.ForegroundColor = ConsoleColor.White;
				}
			}

			end_log_file(done_count, in_danger_count, 0);
		}
	}
}
