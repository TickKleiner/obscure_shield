using System;

namespace obscure_shield
{
	class Program : ParserUtils
	{
		string api_token = "";
		string file_path = "";
		string main_name = "";
		long main_usr_id = 0;
		int flags = 0;

		bool validate(string[] args, int argc)
		{
			if (argc < 3 || argc > 5)
				return (false);
			if ((api_token = get_token(args, argc)) == "")
				return (false);
			if ((flags = get_flags(args, argc)) > 4)
				return (false);

			main_name = get_name(args, argc);
			file_path = get_file_path(args, argc);

			if (main_name == "" && file_path == "")
				return (false);
			if (main_name == "" && flags > 0)
				return (false);

			return (true);
		}
		int Main(string[] args, int argc)
		{
			if (validate(args, argc) == false)
				return (1);

			ApiClient client = new ApiClient();
			Parser parser = new Parser();

			int ret = 0;

			client.api_auth(api_token);

			if (file_path != "")
				if ((ret = client.get_user_id(parse_file(file_path))) > 0)
					return (ret);
				else if (flags == 4)
					if ((ret = client.get_members(main_name)) > 0)
						return (ret);
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


//Flags:
/*
obscure_shield.exe [token][link_to_file with user_links/id/name] проверить аккаунты из файла на слив
obscure_shield.exe [token][user_link/id/name] --friends --followers
											  --friends - проверить всех друзей на слив
														--followers - проверить пописчиков на слив
obscure_shield.exe [token][club_id] --club - проверить подписчиков группы на слив
*/
