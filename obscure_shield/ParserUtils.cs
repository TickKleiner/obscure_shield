using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace obscure_shield
{
	public class ParserUtils
	{
		protected ParserUtils() {}

		string file_name;
		StreamWriter sw;

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

		protected void end_log_file(uint checked_ids_count, uint in_danger_ids_count, ulong check_time)
		{
			sw.WriteLine();
			sw.WriteLine("==============================================================================");
			sw.WriteLine();
			sw.WriteLine("Total accounts checked - " + checked_ids_count.ToString());
			sw.WriteLine("Total accounts in danger - " + in_danger_ids_count.ToString());
			sw.WriteLine(check_time + " s - How long does it take to keep your friends safe");
			sw.Close();
		}

		protected int parse_exception(string message)
		{
			string buf = Regex.Match(message, @"\(\d+\)").Value;
			buf = buf.Trim('(', ')');

			return (Int32.Parse(buf));
		}
	}
}
