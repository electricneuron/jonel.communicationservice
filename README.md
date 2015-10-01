Installation Steps <br />
1. Install Service Using InstallUtil <br />
2. Add Port (508) to Firewall <br />
3. Add URL (http://localhost:508) to ACL List <br />
	i. Run cmd (Elevated) <br />
	ii. Run Command<br />
	   netsh http add urlacl url=http://localhost:508/ user=Everyone <br />
4. Start the Service "Jonel Communicator" <br />
