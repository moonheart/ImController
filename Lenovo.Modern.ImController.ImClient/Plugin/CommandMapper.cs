using System;
using System.Collections.Generic;
using System.Linq;
using Lenovo.Modern.ImController.ImClient.Models;

namespace Lenovo.Modern.ImController.ImClient.Plugin
{
	// Token: 0x02000008 RID: 8
	public class CommandMapper
	{
		// Token: 0x0600001F RID: 31 RVA: 0x00002698 File Offset: 0x00000898
		public CommandMapper()
		{
			this._registeredRequestCommands = new Dictionary<CommandMapper.ContractRequestPair, PluginEntryWrapper.PluginEntryRequestFunction>();
			this._registeredEventCommands = new Dictionary<CommandMapper.EventTriggerpair, PluginEntryWrapper.PluginEntryEventFunction>();
		}

		// Token: 0x06000020 RID: 32 RVA: 0x000026B8 File Offset: 0x000008B8
		public void RegisterRequestCommandHandler(string contractName, string commandName, PluginEntryWrapper.PluginEntryRequestFunction commandHandler)
		{
			if (string.IsNullOrWhiteSpace(commandName) || string.IsNullOrWhiteSpace(contractName) || commandHandler == null)
			{
				throw new ArgumentNullException("Must provide valid arguments for registration");
			}
			if (this._registeredRequestCommands != null)
			{
				if (this._registeredRequestCommands.FirstOrDefault((KeyValuePair<CommandMapper.ContractRequestPair, PluginEntryWrapper.PluginEntryRequestFunction> pair) => pair.Key.Contractname.Equals(contractName, StringComparison.InvariantCultureIgnoreCase) && pair.Key.CommandName.Equals(commandName, StringComparison.InvariantCultureIgnoreCase)).Value != null)
				{
					throw new InvalidOperationException(string.Format("ContractRequest {0}:{1} already registered", contractName, commandName));
				}
				this._registeredRequestCommands.Add(new CommandMapper.ContractRequestPair(contractName, commandName), commandHandler);
			}
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002764 File Offset: 0x00000964
		public PluginEntryWrapper.PluginEntryRequestFunction ResolveRequestCommandHandler(ContractRequest request)
		{
			string commandName = null;
			string contractName = null;
			PluginEntryWrapper.PluginEntryRequestFunction result = null;
			if (request == null || request.Command == null || string.IsNullOrWhiteSpace(request.Command.Name))
			{
				throw new ArgumentNullException("Must provide valid arguments for resolve");
			}
			contractName = request.Name;
			commandName = request.Command.Name;
			if (this._registeredRequestCommands != null)
			{
				KeyValuePair<CommandMapper.ContractRequestPair, PluginEntryWrapper.PluginEntryRequestFunction> keyValuePair = this._registeredRequestCommands.FirstOrDefault((KeyValuePair<CommandMapper.ContractRequestPair, PluginEntryWrapper.PluginEntryRequestFunction> pair) => pair.Key.Contractname.Equals(contractName, StringComparison.InvariantCultureIgnoreCase) && pair.Key.CommandName.Equals(commandName, StringComparison.InvariantCultureIgnoreCase));
				if (keyValuePair.Value == null)
				{
					throw new InvalidOperationException(string.Format("Command {0}:{1} not registered", contractName, commandName));
				}
				result = keyValuePair.Value;
			}
			return result;
		}

		// Token: 0x06000022 RID: 34 RVA: 0x0000281C File Offset: 0x00000A1C
		public void RegisterEventHandler(string eventMonitorName, string eventTriggerName, PluginEntryWrapper.PluginEntryEventFunction eventHandler)
		{
			if (string.IsNullOrWhiteSpace(eventMonitorName) || string.IsNullOrWhiteSpace(eventTriggerName) || eventHandler == null)
			{
				throw new ArgumentNullException("Must provide valid arguments for registration");
			}
			if (this._registeredEventCommands != null)
			{
				if (this._registeredEventCommands.FirstOrDefault((KeyValuePair<CommandMapper.EventTriggerpair, PluginEntryWrapper.PluginEntryEventFunction> pair) => pair.Key.EventMonitor.Equals(eventMonitorName, StringComparison.InvariantCultureIgnoreCase) && pair.Key.EventTrigger.Equals(eventTriggerName, StringComparison.InvariantCultureIgnoreCase)).Value != null)
				{
					throw new InvalidOperationException(string.Format("Event {0}:{1} already registered", eventMonitorName, eventTriggerName));
				}
				this._registeredEventCommands.Add(new CommandMapper.EventTriggerpair(eventMonitorName, eventTriggerName), eventHandler);
			}
		}

		// Token: 0x06000023 RID: 35 RVA: 0x000028C8 File Offset: 0x00000AC8
		public PluginEntryWrapper.PluginEntryEventFunction ResolveEventHandler(string eventMonitorName, string eventTriggerName)
		{
			PluginEntryWrapper.PluginEntryEventFunction result = null;
			if (string.IsNullOrWhiteSpace(eventMonitorName) || string.IsNullOrWhiteSpace(eventTriggerName))
			{
				throw new ArgumentNullException("Must provide valid arguments for resolve");
			}
			if (this._registeredEventCommands != null)
			{
				KeyValuePair<CommandMapper.EventTriggerpair, PluginEntryWrapper.PluginEntryEventFunction> keyValuePair = this._registeredEventCommands.FirstOrDefault((KeyValuePair<CommandMapper.EventTriggerpair, PluginEntryWrapper.PluginEntryEventFunction> pair) => pair.Key.EventMonitor.Equals(eventMonitorName, StringComparison.InvariantCultureIgnoreCase) && pair.Key.EventTrigger.Equals(eventTriggerName, StringComparison.InvariantCultureIgnoreCase));
				if (keyValuePair.Value == null)
				{
					throw new InvalidOperationException(string.Format("Event {0}:{1} not registered", eventMonitorName, eventTriggerName));
				}
				result = keyValuePair.Value;
			}
			return result;
		}

		// Token: 0x04000009 RID: 9
		private Dictionary<CommandMapper.ContractRequestPair, PluginEntryWrapper.PluginEntryRequestFunction> _registeredRequestCommands;

		// Token: 0x0400000A RID: 10
		private Dictionary<CommandMapper.EventTriggerpair, PluginEntryWrapper.PluginEntryEventFunction> _registeredEventCommands;

		// Token: 0x0200003E RID: 62
		private class EventTriggerpair
		{
			// Token: 0x06000142 RID: 322 RVA: 0x00004E37 File Offset: 0x00003037
			public EventTriggerpair(string eventMonitorName, string eventTriggerName)
			{
				this.EventMonitor = eventMonitorName;
				this.EventTrigger = eventTriggerName;
			}

			// Token: 0x1700004B RID: 75
			// (get) Token: 0x06000143 RID: 323 RVA: 0x00004E4D File Offset: 0x0000304D
			// (set) Token: 0x06000144 RID: 324 RVA: 0x00004E55 File Offset: 0x00003055
			public string EventMonitor { get; private set; }

			// Token: 0x1700004C RID: 76
			// (get) Token: 0x06000145 RID: 325 RVA: 0x00004E5E File Offset: 0x0000305E
			// (set) Token: 0x06000146 RID: 326 RVA: 0x00004E66 File Offset: 0x00003066
			public string EventTrigger { get; private set; }
		}

		// Token: 0x0200003F RID: 63
		public class ContractRequestPair
		{
			// Token: 0x06000147 RID: 327 RVA: 0x00004E6F File Offset: 0x0000306F
			public ContractRequestPair(string contractName, string commandName)
			{
				this.Contractname = contractName;
				this.CommandName = commandName;
			}

			// Token: 0x1700004D RID: 77
			// (get) Token: 0x06000148 RID: 328 RVA: 0x00004E85 File Offset: 0x00003085
			// (set) Token: 0x06000149 RID: 329 RVA: 0x00004E8D File Offset: 0x0000308D
			public string Contractname { get; private set; }

			// Token: 0x1700004E RID: 78
			// (get) Token: 0x0600014A RID: 330 RVA: 0x00004E96 File Offset: 0x00003096
			// (set) Token: 0x0600014B RID: 331 RVA: 0x00004E9E File Offset: 0x0000309E
			public string CommandName { get; private set; }
		}
	}
}
