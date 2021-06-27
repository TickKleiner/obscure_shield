using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace obscure_shield
{
	public class ParserUtils
	{
		public ParserUtils() {}

		private string file_name;
		private StreamWriter sw;

		protected uint get_acc_count(string page)
		{
			uint count = 0;

			string buf = Regex.Match(page, @"Show \d+ account").Value;
			buf = Regex.Match(buf, @"\d+").Value;
			count = uint.Parse(buf);

			return (count);
		}

		protected void create_res_file()
		{
			string path = @".\results\res_";
			string extension = @".txt";
			uint file_num = 0;

			while (File.Exists(path + file_num.ToString() + extension))
				++file_num;

			file_name = path + file_num.ToString() + extension;

			string dir = @".\results\";

			if (!Directory.Exists(dir))
			{
				Directory.CreateDirectory(dir);
			}

			sw = File.AppendText(file_name);
		}

		protected void add_str_to_log(string link, uint count)
		{
			sw.WriteLine(link + " in danger, number of occurrences = " + count.ToString());
		}

		protected void end_log_file(uint checked_ids_count, uint in_danger_ids_count, TimeSpan check_time)
		{
			sw.WriteLine();
			sw.WriteLine("=================================================================================================");
			sw.WriteLine();
			sw.WriteLine("Total accounts checked - " + checked_ids_count.ToString());
			sw.WriteLine("Total accounts in danger - " + in_danger_ids_count.ToString());
			sw.WriteLine(check_time.ToString() + " s - How long does it take to keep your friends safe");
			sw.Close();
		}

		protected int parse_exception(string message)
		{
			string buf = Regex.Match(message, @"\(\d+\)").Value;
			buf = buf.Trim('(', ')');

			return (Int32.Parse(buf));
		}

		public List<string> parse_file(string path)
		{
			List<string> names = new List<string>();

			string[] lines = File.ReadAllLines(path);

			foreach (string line in lines)
			{
				string buf = line.Remove(0, line.LastIndexOf('/') + 1);
				names.Add(buf);
			}

			return (names);
		}

		public string get_token(string[] argv, int argc)
		{
			string token = "";

			foreach (string arg in argv)
			{
				if (Regex.IsMatch(arg, @"[0-9a-z]{71}"))
				{
					token = arg;
					break;
				}
			}

			return (token);
		}

		public string get_file_path(string[] argv, int argc)
		{
			string path = "";

			foreach (string arg in argv)
			{
				if (File.Exists(arg))
				{
					path = arg;
					break;
				}
			}

			return (path);
		}

		public int get_flags(string[] argv, int argc)
		{
			///
			///	000 = 0 - no flags
			///	100 = 1 - "--friends"
			///	010 = 2 - "--followers"
			///	001 = 4 - "--club"
			///

			int ret = 0;

			foreach (string arg in argv)
			{
				if (arg.ToLower() == "--friends")
					ret += 1;
				else if (arg.ToLower() == "--followers")
					ret += 2;
				else if (arg.ToLower() == "--club")
					ret += 4;
			}

			return (ret);
		}

		public string get_name(string[] argv, int argc)
		{
			string id = "";

			foreach (string arg in argv)
			{
				if (arg.StartsWith("https://vk.com/") ||
					arg.StartsWith("http://vk.com/") ||
					arg.StartsWith("https://m.vk.com/") ||
					arg.StartsWith("http://m.vk.com/"))
					id = arg.Remove(0, arg.LastIndexOf('/') + 1);
			}

			return (id);
		}
	}
}
