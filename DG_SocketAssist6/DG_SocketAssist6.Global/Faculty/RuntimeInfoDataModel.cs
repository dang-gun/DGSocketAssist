using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DG_SocketAssist6.Global.Faculty
{
    /// <summary>
    /// https://github.com/jmalarcon/DotNetVersions
    /// https://github.com/EliteLoser/DotNetVersionLister
    /// </summary>
    /// <remarks>
    /// NET Core 3.0이상에서만 사용
    /// </remarks>
    public class RuntimeInfoDataModel
    {
        /// <summary>
        /// .NET 정보
        /// </summary>
        public FrameworkInfoDataModel FrameworkInfo { get; private set; }

        /// <summary>
        /// 운영체제 정보
        /// </summary>
        public OsInfoDataModel OsInfo { get; private set; }

        public RuntimeInfoDataModel()
        {
            this.FrameworkInfo = new FrameworkInfoDataModel();
            this.OsInfo = new OsInfoDataModel();
        }

    }

    /// <summary>
    /// .NET 정보
    /// </summary>
    public class FrameworkInfoDataModel
    {
        /// <summary>
        /// 이름
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// 버전 문자열
        /// </summary>
        public string Version { get; set; } = string.Empty;

        /// <summary>
        /// 버전 - 주버전
        /// </summary>
        public int Major { get; set; } = 0;
        /// <summary>
        /// 버전 - 서비스팩
        /// </summary>
        public int ServicePack { get; set; } = 0;
        /// <summary>
        /// 버전 - 핫픽스
        /// </summary>
        public int Hotfix { get; set; } = 0;

        /// <summary>
        /// 데이터 모델을 생성한다.
        /// </summary>
        public FrameworkInfoDataModel()
        {
            //버전 전체 정보
            string frameworkDescription 
                = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription;

            //최초의 빈칸 위치를 찾는다.
            int startIndex = frameworkDescription.IndexOf(' ');
            if (startIndex > 0)
            {

                //이름 추출
                this.Name = frameworkDescription.Substring(0, startIndex);
                //버전 문자열 추출
                this.Version = frameworkDescription.Substring(startIndex + 1);

                //버전 문자열을 분리하여 숫자로 변환
                string[] sVerStrCut = this.Version.Split('.');
                if(1 <= sVerStrCut.Length)
                {
                    this.Major = Convert.ToInt32(sVerStrCut[0]);
                }
                if (2 <= sVerStrCut.Length)
                {
                    this.ServicePack = Convert.ToInt32(sVerStrCut[1]);
                }
                if (3 <= sVerStrCut.Length)
                {
                    this.Hotfix = Convert.ToInt32(sVerStrCut[2]);
                }
            }
        }
    }

    /// <summary>
    /// OS 구분 타입
    /// </summary>
    /// <remarks>
    /// OperatingSystem를 기준으로 작성함
    /// <para>OperatingSystem구현을 타입으로 사용하는 구현이 있을 법한데 찾지를 못해서 직접구현함.<br />
    /// 나중에라도 찾는다면 지운다.</para>
    /// </remarks>
    public enum OsType
    {
        /// <summary>
        /// 알 수 없음
        /// </summary>
        Other = 0,

        /// <summary>
        /// Indicates whether the current application is running as WASM in a Browser.
        /// </summary>
        Browser,

        /// <summary>
        /// Indicates whether the current application is running on Linux.
        /// </summary>
        Linux,

        /// <summary>
        /// Indicates whether the current application is running on FreeBSD.
        /// </summary>
        FreeBSD,

        /// <summary>
        /// Indicates whether the current application is running on Android.
        /// </summary>
        Android,

        /// <summary>
        /// Indicates whether the current application is running on iOS or MacCatalyst.
        /// </summary>
        IOS,

        /// <summary>
        /// Indicates whether the current application is running on macOS.
        /// </summary>
        MacOS,

        /// <summary>
        /// Indicates whether the current application is running on Mac Catalyst.
        /// </summary>
        MacCatalyst,

        /// <summary>
        /// Indicates whether the current application is running on tvOS.
        /// </summary>
        TvOS,

        /// <summary>
        /// Check for the tvOS version (returned by 'libobjc.get_operatingSystemVersion') with a >= version comparison. Used to guard APIs that were added in the given tvOS release.
        /// </summary>
        WatchOS,

        /// <summary>
        /// Indicates whether the current application is running on Windows.
        /// </summary>
        Windows,
    }

    /// <summary>
    /// OS 정보 데이터 모델
    /// </summary>
    public class OsInfoDataModel
    {
        /// <summary>
        /// 플랫폼(운영체제) ID
        /// </summary>
        /// <remarks>
        /// Environment.OSVersion의 내용
        /// </remarks>
        public PlatformID OsId { get; set; }
        /// <summary>
        /// 플랫폼(운영체제) 버전
        /// </summary>
        /// <remarks>
        /// Environment.OSVersion의 내용
        /// </remarks>
        public Version Version { get; set; }

        /// <summary>
        /// 운영체제 정보 문자열
        /// </summary>
        /// <remarks>
        /// System.Runtime.InteropServices.RuntimeInformation.OSDescription
        /// </remarks>
        public string OsString { get; set; }

        /// <summary>
        /// 구분된 OS 타입
        /// </summary>
        public OsType OsTyep { get; set; } = OsType.Other;

        /// <summary>
        /// 데이터 모델을 생성한다.
        /// </summary>
        public OsInfoDataModel()
        {
            OperatingSystem temp = Environment.OSVersion;
            this.OsId = temp.Platform;
            this.Version = temp.Version;

            //버전 문자열 자르기
            //Microsoft Windows 10.0.19045
            //Ubuntu 16.04 LTS (Xenial Xerus)
            //macOS 14.0 Sonoma, Darwin 23.0.0 Darwin Kernel Version 23.0.0: Fri Feb 3 10:41:57 PST 2024; root:xnu-8480.200.4~1/RELEASE_X86_64
            this.OsString
                = System.Runtime.InteropServices.RuntimeInformation.OSDescription;


            //TARGET_[OS]로 구현할까 하다가
            //OperatingSystem가 변경될때 마다 같이 관리할 자신이 없어서 이렇게 구현한다.
            if(OperatingSystem.IsBrowser()) { this.OsTyep = OsType.Browser; }
            else if (OperatingSystem.IsLinux()) { this.OsTyep = OsType.Linux; }
            else if (OperatingSystem.IsFreeBSD()) { this.OsTyep = OsType.FreeBSD; }
            else if (OperatingSystem.IsAndroid()) { this.OsTyep = OsType.Android; }
            else if (OperatingSystem.IsIOS()) { this.OsTyep = OsType.IOS; }
            else if (OperatingSystem.IsMacOS()) { this.OsTyep = OsType.MacOS; }
            else if (OperatingSystem.IsMacCatalyst()) { this.OsTyep = OsType.MacCatalyst; }
            else if (OperatingSystem.IsTvOS()) { this.OsTyep = OsType.TvOS; }
            else if (OperatingSystem.IsWatchOS()) { this.OsTyep = OsType.WatchOS; }
            else if (OperatingSystem.IsWindows()) { this.OsTyep = OsType.Windows; }
        }
    }
}
