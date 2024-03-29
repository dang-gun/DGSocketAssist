# DGSocketAssist1 (.NET Framework 4.8)
C# Socket + SocketAsyncEventArgs 사용을 이해하기 위한 라이브러리
C#에서 C# Socket과 SocketAsyncEventArgs를 사용할때 메커니즘을 공부할때 사용하라고 만든 라이브러리입니다.

'DGSocketAssist1_Server', 'DGSocketAssist1_Client'(혹은 'DGSocketAssist1_Client_Upgrade') 만 참조하여 서버/클라이언트 프로그램을 만들 수 있습니다.

.NET Framework 4.8으로 구성되어 있습니다.<br />
.NET5의 경우 'SocketAsyncEventArgs'가 최적화 되면서 기존버전과 다르게 동작하는 것이 확인 되었습니다.<br />
(참고 - https://github.com/dotnet/runtime/issues/24950#issuecomment-364621160)<br />
<br />

이것에 관한 처리를 추가로 한것이 'DGSocketAssist1_Client_Upgrade'입니다.<br />
'DGSocketAssist1_Client'와 'DGSocketAssist1_Client_Upgrade'를 번갈아가며 테스트해보길 바랍니다.
(참고 : [[.NET 5] 달라진 'SocketAsyncEventArgs'의 동작 때문에 'Completed' 이벤트가 오지 않는 현상 해결하기](https://blog.danggun.net/11035))



이 프로젝트는 학습을 위한 프로젝트로 특별한 이유가 없는 이상 업데이트를 하지 않을 예정입니다.<br />
학습을 위해 만들어진 코드이므로 충분한 테스트가 되지 않습니다.<br />
(버그 주의!)<br />
<br />
이 프로젝트는 모든 데이터를 문자열로 처리합니다.<br />
<br />
<br />
<br />

### 프로젝트 구성
.NET Framework 4.8<br />
VisualStudio 2022<br />
<br />
<br />
Socket, SocketAsyncEventArgs를 이해하기 위한 라이브러리<br />
- DGSocketAssist1_Server<br />
- DGSocketAssist1_Client<br />
- DGSocketAssist1_Client_Upgrade<br />
<br />
<br />
이 라이브러리를 이용하여 만든 체팅 셈플<br />
- SocketServerTest<br />
- SocketClientTest<br />
- SocketClientNet5Test(.NET 5)<br />
