using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.FNode
{
    public abstract class NodeBase : Node
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
        TextField _guidText;
        public NodeBase(string nodeName)
        {
            GUID = System.Guid.NewGuid().ToString();
            title = nodeName;
            _guidText = new TextField("GUID");
            _guidText.value = GUID;
            this.mainContainer.Add(_guidText);
            _guidText.SendToBack();
        }




        /// <summary>
        /// 尝试获取输出端口
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Port TryGetOutputPort(string name)
        {
            foreach (var element in this.outputContainer.Children())
                if (element is Port port)
                    if (port.name == name)
                        return port;
            return null;
        }
        /// <summary>
        /// 尝试获取输入端口
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Port TryGetInputPort(string name)
        {
            foreach (var element in this.inputContainer.Children())
                if (element is Port port)
                    if (port.name == name)
                        return port;
            return null;
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
        /// 添加元素
        /// </summary>
        /// <param name="child"></param>
        public void AddContent(VisualElement child)
        {
            this.mainContainer.Add(child);
        }


        public abstract void DeserializeJson(string json);
        public abstract string SerializeJson();
    }
}
