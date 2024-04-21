using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace FNode.Editor
{

    public interface INodeFieldsSerializeBehaviour
    {
        public void InternalDeserialize(string json);
        public string InternalSerialize();
    }
    public abstract class NodeBase : Node, IUnique,INodeFieldsSerializeBehaviour
    {
        private string _guid;

        /// <summary>
        /// 节点全局唯一标识
        /// </summary>
        public string GUID
        {
            get => _guid;
            set
            {
                _guid = value;
                if (_guidText != null)
                    _guidText.value = value;
            }
        }

        public string UniqueName { get; set; } = string.Empty;

        /// <summary>
        /// 节点位置
        /// </summary>
        public Vector2 Position
        {
            get => this.GetPosition().position;
            set
            {
                this.SetPosition(new Rect(value, Vector2.zero));
            }
        }

        /// <summary>
        /// 节点GUID显示框
        /// </summary>
        private readonly TextField _guidText;
        internal NodeBase(string nodeName)
        {
            GUID = System.Guid.NewGuid().ToString();
            title = nodeName;
            _guidText = new TextField("GUID");
            _guidText.value = GUID;
            this.mainContainer.Add(_guidText);
            _guidText.SendToBack();
            _guidText.SetEnabled(false);
        }

        /// <summary>
        /// 尝试获取输出端口
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool TryGetOutputPort(string name, out Port existedPort)
        {
            foreach (var element in this.outputContainer.Children())
                if (element is Port port)
                    if (port.name == name)
                    {
                        existedPort = port;
                        return true;
                    }

            existedPort = null;
            return false;
        }
        /// <summary>
        /// 尝试获取输入端口
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool TryGetInputPort(string name, out Port existedPort)
        {
            foreach (var element in this.inputContainer.Children())
                if (element is Port port)
                    if (port.name == name)
                    {
                        existedPort = port;
                        return true;
                    }
            existedPort = null;
            return false;
        }

        /// <summary>
        /// 创建输入端口
        /// </summary>
        /// <param name="portName"></param>
        /// <param name="type"></param>
        /// <param name="mutiple"></param>
        public void CreateInputPort(string portName, string uniqueName, Type type, bool mutiple = false)
        {
            var inputPort = Port.Create<Edge>(Orientation.Horizontal,
              Direction.Input,
              mutiple ? Port.Capacity.Multi : Port.Capacity.Single,
              type);

            inputPort.portName = portName;
            inputPort.name = uniqueName;

            inputContainer.Add(inputPort);

            RefreshExpandedState();
        }
        /// <summary>
        /// 创建输出端口
        /// </summary>
        /// <param name="portName"></param>
        /// <param name="type"></param>
        /// <param name="mutiple"></param>
        public void CreateOutputPort(string portName, string uniqueName, Type type, bool mutiple = false)
        {
            var outPort = Port.Create<Edge>(Orientation.Horizontal,
                Direction.Output,
                mutiple ? Port.Capacity.Multi : Port.Capacity.Single,
                type);
            outPort.portName = portName;
            outPort.name = uniqueName;
            outputContainer.Add(outPort);

            RefreshExpandedState();
        }

        /// <summary>
        /// 在内容中添加
        /// </summary>
        /// <param name="child"></param>
        public void AddContent(VisualElement child)
        {
            this.contentContainer.Add(child);
        }



        protected abstract void OnDeserialize(string json);
        protected abstract string OnSerialize();

        void INodeFieldsSerializeBehaviour.InternalDeserialize(string json) => OnDeserialize(json);

        string INodeFieldsSerializeBehaviour.InternalSerialize() => OnSerialize();
    }
}
