using System.Collections.Generic;
using System.Linq;
using MergeCase.General.Interfaces;
using MergeCase.Systems.Command.Interfaces;
using MergeCase.Systems.Gameplay;

namespace MergeCase.Systems.Command
{
    public class CommandSystem : GameplaySystemBase, ICommandCollection, IInitializable, IUpdateable
    {
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ShowInInspector]
#endif
        List<ICommand> _commands = new();

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ShowInInspector]
#endif
        List<ICommand> _commandsToRemove = new();

        public bool TryInitialize()
        {
            _commands ??= new();
            _commandsToRemove ??= new();
            return true;
        }

        public bool TryDeInitialize()
        {
            _commands?.Clear();
            _commandsToRemove?.Clear();
            return true;
        }

        public bool TryAdd<T>(T command) where T : ICommand
        {
            if (command == null)
            {
                UnityLogger.LogErrorWithTag("Trying to add a null command will result in failure!");
                return false;
            }

            if (!command.TryInitialize())
            {
                UnityLogger.LogErrorWithTag($"Command : {command} failed to initailize! Cannot add to command system!");
                return false;
            }

            _commands.Add(command);

            return true;
        }

        public bool TryRemove<T>(T command) where T : ICommand
        {
            if (!_commands.Remove(command))
            {
                UnityLogger.LogErrorWithTag($"Command : {command} is no present in the system! Cannot remove command!");
                return false;
            }

            return true;
        }

        public bool TryGet<T>(out T command) where T : ICommand
        {
            command = (T)_commands.Find(x => x.GetType() == typeof(T));

            if (command == null)
            {
                UnityLogger.LogErrorWithTag($"Command : {command} is no present in the system! Cannot get non existing command!");
                return false;
            }

            return true;
        }

        public bool TryGetAll<T>(out ICollection<T> commands) where T : ICommand
        {
            commands = (ICollection<T>)_commands.FindAll(x => x.GetType() == typeof(T)).ToList();
            return commands != null && commands.Count > 0;
        }

        public bool TryGetAllNoAlloc<T>(ICollection<T> commands) where T : ICommand
        {
            commands.Clear();

            foreach (var command in _commands)
            {
                if (command is not T commanType)
                {
                    continue;
                }

                commands.Add((T)command);
            }

            return commands.Count > 0;
        }

        public bool TryUpdate()
        {
            foreach (var command in _commands)
            {
                if (command is null)
                {
                    UnityLogger.LogErrorWithTag($"A command is null! Skipping!");
                    continue;
                }

                if (!command.TryUpdate())
                {
                    UnityLogger.LogErrorWithTag($"A command failed to update! Skipping!");
                    continue;
                }

                if (command.IsCompleted())
                {
                    _commandsToRemove.Add(command);
                }
            }

            foreach (var command in _commandsToRemove)
            {
                _commands.Remove(command);
            }

            _commandsToRemove.Clear();
            _commands.RemoveAll(x => x == null);

            return true;
        }
    }
}

