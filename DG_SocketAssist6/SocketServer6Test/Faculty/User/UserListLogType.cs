
using System.ComponentModel.DataAnnotations;

namespace SocketServer6Test.Faculty.User;

/// <summary>
/// 유저 리스트 모델에서 사용하는 로그 타입
/// </summary>
public enum UserListLogType
{
    None = 0,

    /// <summary>
    /// 유저 상태 관련
    /// </summary>
    UserState = 100,
    

    Max = int.MaxValue,
}
