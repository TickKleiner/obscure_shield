using System;

namespace obscure_shield
{
	class Program
	{
		static string api_token = "";
		static string file_path = "";
		static string main_name = "";
		static long main_usr_id = 0;
		static int flags = 0;

		static bool validate(string[] args, ParserUtils utils)
		{
			if (args.Length < 3 || args.Length > 5)
				return (false);
			if ((api_token = utils.get_token(args, args.Length)) == "")
				return (false);
			if ((flags = utils.get_flags(args, args.Length)) > 4)
				return (false);

			main_name = utils.get_name(args, args.Length);
			file_path = utils.get_file_path(args, args.Length);

			if (main_name == "" && file_path == "")
				return (false);
			if (main_name == "" && flags > 0)
				return (false);

			return (true);
		}
		private static int Main(string[] args)
		{
			ParserUtils utils = new ParserUtils();

			if (validate(args, utils) == false)
				return (1);
			ApiClient client = new ApiClient();
			Parser parser = new Parser();

			int ret = 0;

			client.api_auth(api_token);
			if (file_path != "")
			{
				if ((ret = client.get_user_id(utils.parse_file(file_path))) > 0)
				{
					return (ret);
				}
			}
			else if (flags == 4)
			{
				if ((ret = client.get_members(main_name)) > 0)
				{
					return (ret);
				}
			}
			else
			{
				if ((ret = client.get_user_id(main_name)) > 0)
					return (ret);
				main_usr_id = client.id_s[0];
				if ((flags == 1 || flags == 3) && (ret = client.get_friends(main_usr_id)) > 0)
					return (ret);
				if ((flags == 2 || flags == 3) && (ret = client.get_followers(main_usr_id)) > 0)
					return (ret);
			}

			parser.parse_id_s(client.id_s);

			Console.WriteLine("Press ay key to exit...");
			Console.ReadKey();
			return (0);
		}
	}
}
