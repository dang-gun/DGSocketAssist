# DGSocketAssist2 (.NET Framework 4.8)
C# Socket + SocketAsyncEventArgs 사용을 이해하기 위한 라이브러리 두번째 버전<br />
C#에서 C# Socket과 SocketAsyncEventArgs를 사용할때 메커니즘을 공부할때 사용하라고 만든 라이브러리입니다.<br />
<br />
"DGSocketAssist2_Server", "DGSocketAssist2_Client" 만 참조하여 서버/클라이언트 프로그램을 만들 수 있습니다.<br />
<br />
기존 프로젝트가 문자열로만 처리하던 것을 바이너리(byte[])처리하도록 변경하였습니다.<br />
그로 인해 샘플용 체팅 프로그램은 'ChatSetting.ByteArrayToString', 'ChatSetting.StringToByteArray'를 이용하여 문자열을 디코딩/인코딩 하여 사용하도록 변경되었습니다.<br />
<br />

.NET Framework 4.8으로 구성되어 있습니다.<br />
.NET5의 경우 'SocketAsyncEventArgs'가 최적화 되면서 기존버전과 다르게 동작하는 것이 확인 되었습니다.<br />
(참고 - https://github.com/dotnet/runtime/issues/24950#issuecomment-364621160)<br />

이 프로젝트는 학습을 위한 프로젝트로 특별한 이유가 없는 이상 업데이트를 하지 않을 예정입니다.
학습을 위해 만들어진 코드이므로 충분한 테스트가 되지 않습니다.<br />
(버그 주의!)<br />
<br />
<br />

### 프로젝트 구성
.NET Framework 4.8<br />
VisualStudio 2022<br />
<br />
<br />
Socket, SocketAsyncEventArgs를 이해하기 위한 라이브러리<br />
- DGSocketAssist2_Server<br />
- DGSocketAssist2_Client<br />
<br />
<br />
이 라이브러리를 이용하여 만든 체팅 셈플<br />
- SocketServerTest<br />
- SocketClientTest<br />
<br />
