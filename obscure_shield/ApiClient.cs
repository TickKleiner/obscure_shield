using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Model.RequestParams;
using System.Text.RegularExpressions;

using USRS_CONT = VkNet.Utils.VkCollection<VkNet.Model.User>;
using USR_CONT= System.Collections.ObjectModel.ReadOnlyCollection<VkNet.Model.User>;

namespace obscure_shield
{
	class ApiClient
	{
		VkApi api = new VkApi();
		public List<long> id_s { get; private set; }

		ApiClient() {}

		private int parse_exception(string msg)
		{
			string buf = Regex.Match(msg, @"\(\d+\)").Value;
			buf = buf.Trim('(', ')');

			return (Int32.Parse(buf));
		}

		private void fill_id_s(IEnumerable<VkNet.Model.User> resp)
		{
			foreach (var usr in resp)
				id_s.Append(usr.Id);
		}

		public void api_auth(string _login, string _password, ulong _app_id)
		{
			api.Authorize(new VkNet.Model.ApiAuthParams()
			{
				Login = _login,
				Password = _password,
				ApplicationId = _app_id
			});
		}

		public void api_auth(string _token)
		{
			api.Authorize(new VkNet.Model.ApiAuthParams()
			{
				AccessToken = _token
			});
		}

		public int get_user_id(string user_name)
		{
			USR_CONT resp;

			List<string> user_id = new List<string>();
			user_id.Append(user_name);

			try
			{
				resp = api.Users.Get(user_id);
			}
			catch(Exception e)
			{
				return parse_exception(e.Message);
			}

			fill_id_s(resp);

			return 0;
		}

		public int get_user_id(List<string> user_names)
		{
			USR_CONT resp;

			try
			{
				resp = api.Users.Get(user_names);
			}
			catch(Exception e)
			{
				return parse_exception(e.Message);
			}

			fill_id_s(resp);

			return 0;
		}

		public int get_followers(long user_id)
		{
			int count = 1000;
			USRS_CONT resp;

			try
			{
				resp = api.Users.GetFollowers(user_id, count);
			}
			catch(Exception e)
			{
				return parse_exception(e.Message);
			}

			fill_id_s(resp);

			return 0;
		}

		public int get_friends(long user_id)
		{
			USRS_CONT resp;

			try
			{
				resp = api.Friends.Get(new VkNet.Model.RequestParams.FriendsGetParams()
				{
					UserId = user_id,
					Count = 5000
				});
			}
			catch(Exception e)
			{
				return parse_exception(e.Message);
			}

			fill_id_s(resp);

			return 0;
		}

		public int get_members(string group_id)
		{
			USRS_CONT resp;

			try
			{
				resp = api.Groups.GetMembers(new VkNet.Model.RequestParams.GroupsGetMembersParams()
				{
					GroupId = group_id,
					Count = 1000
				});
			}
			catch(Exception e)
			{
				return parse_exception(e.Message);
			}

			fill_id_s(resp);

			return 0;
		}
	}
}
