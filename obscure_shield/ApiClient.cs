using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Model.RequestParams;

using USRCONTAINER = VkNet.Utils.VkCollection<VkNet.Model.User>;

namespace obscure_shield
{
	class ApiClient
	{
		VkApi api = new VkApi();
		ApiClient()
		{

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
		
		public void get_user_id(string user_name)
		{
			List<string> user_id = new List<string>();
			user_id.Append(user_name);
			var resp = api.Users.Get(user_id);
		}

		public void get_user_id(List<string> user_names)
		{
			var resp = api.Users.Get(user_names);
		}

		public void get_followers(long user_id, int _count, int _offset)
		{
			var resp = api.Users.GetFollowers(user_id, _count, _offset);
		}

		public void get_followers(long user_id)
		{
			var resp = api.Users.GetFollowers(user_id);
		}

		public void get_friends(long user_id, long _count, long _offset)
		{
			var resp = api.Friends.Get(new VkNet.Model.RequestParams.FriendsGetParams()
			{
				UserId = user_id,
				Count = _count,
				Offset = _offset
			});
		}

		public void get_friends(long user_id)
		{
			var resp = api.Friends.Get(new VkNet.Model.RequestParams.FriendsGetParams()
			{
				UserId = user_id
			});
		}

		public void get_members(string group_id, long _count, long _offset)
		{
			var resp = api.Groups.GetMembers(new VkNet.Model.RequestParams.GroupsGetMembersParams()
			{
				GroupId = group_id,
				Count = _count,
				Offset = _offset
			});
		}

		public void get_members(string group_id)
		{
			var resp = api.Groups.GetMembers(new VkNet.Model.RequestParams.GroupsGetMembersParams()
			{
				GroupId = group_id,
			});
		}
	}
}
