using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Model.RequestParams;

using USRS_CONT = VkNet.Utils.VkCollection<VkNet.Model.User>;
using USR_CONT= System.Collections.ObjectModel.ReadOnlyCollection<VkNet.Model.User>;

namespace obscure_shield
{
	class ApiClient : ParserUtils
	{
		VkApi api = new VkApi();
		public List<long> id_s { get; private set; } = new List<long>();

		public ApiClient() {}

		private void fill_id_s(IEnumerable<VkNet.Model.User> resp)
		{
			foreach (var usr in resp)
				id_s.Add(usr.Id);
		}

		public void api_auth(string _token)
		{
			api.Authorize(new VkNet.Model.ApiAuthParams()
			{
				AccessToken = _token
			});
		}

		public long get_user_id(string user_adds)
		{
			
			USR_CONT resp;

			int ind = 0;
			if ( (ind = user_adds.LastIndexOf('/')) != -1)
				user_adds = user_adds.Substring(0, ind);

			List<string> user_id = new List<string>();
			user_id.Add(user_adds);

			try
			{
				resp = api.Users.Get(user_id);
			}
			catch(Exception e)
			{
				return -1;
			}

			var usr = resp.First();

			return usr.Id;
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
