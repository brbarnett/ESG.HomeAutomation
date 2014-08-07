/*
M2Mqtt Project - MQTT Client Library for .Net and GnatMQ MQTT Broker for .NET
Copyright (c) 2014, Paolo Patierno, All rights reserved.

Licensed under the Apache License, Version 2.0 (the ""License""); you may not use this 
file except in compliance with the License. You may obtain a copy of the License at 
http://www.apache.org/licenses/LICENSE-2.0

THIS CODE IS PROVIDED *AS IS* BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, 
EITHER EXPRESS OR IMPLIED, INCLUDING WITHOUT LIMITATION ANY IMPLIED WARRANTIES OR 
CONDITIONS OF TITLE, FITNESS FOR A PARTICULAR PURPOSE, MERCHANTABLITY OR 
NON-INFRINGEMENT.

See the Apache Version 2.0 License for specific language governing permissions and 
limitations under the License.
*/

#if SSL && !WINDOWS_PHONE
#if (MF_FRAMEWORK_VERSION_V4_2 || MF_FRAMEWORK_VERSION_V4_3)
using Microsoft.SPOT.Net.Security;
#else
using System.Net.Security;
#endif
#endif
using System.Text;

namespace ESG.HomeAutomation.MqttBroker.Messages
{
    /// <summary>
    /// Base class for all MQTT messages
    /// </summary>
    public abstract class MqttMsgBase
    {
        #region Constants...

        // mask, offset and size for fixed header fields
        internal const byte MSG_TYPE_MASK = 0xF0;
        internal const byte MSG_TYPE_OFFSET = 0x04;
        internal const byte MSG_TYPE_SIZE = 0x04;
        internal const byte DUP_FLAG_MASK = 0x08;
        internal const byte DUP_FLAG_OFFSET = 0x03;
        internal const byte DUP_FLAG_SIZE = 0x01;
        internal const byte QOS_LEVEL_MASK = 0x06;
        internal const byte QOS_LEVEL_OFFSET = 0x01;
        internal const byte QOS_LEVEL_SIZE = 0x02;
        internal const byte RETAIN_FLAG_MASK = 0x01;
        internal const byte RETAIN_FLAG_OFFSET = 0x00;
        internal const byte RETAIN_FLAG_SIZE = 0x01;

        // MQTT message types
        internal const byte MQTT_MSG_CONNECT_TYPE = 0x01;
        internal const byte MQTT_MSG_CONNACK_TYPE = 0x02;
        internal const byte MQTT_MSG_PUBLISH_TYPE = 0x03;
        internal const byte MQTT_MSG_PUBACK_TYPE = 0x04;
        internal const byte MQTT_MSG_PUBREC_TYPE = 0x05;
        internal const byte MQTT_MSG_PUBREL_TYPE = 0x06;
        internal const byte MQTT_MSG_PUBCOMP_TYPE = 0x07;
        internal const byte MQTT_MSG_SUBSCRIBE_TYPE = 0x08;
        internal const byte MQTT_MSG_SUBACK_TYPE = 0x09;
        internal const byte MQTT_MSG_UNSUBSCRIBE_TYPE = 0x0A;
        internal const byte MQTT_MSG_UNSUBACK_TYPE = 0x0B;
        internal const byte MQTT_MSG_PINGREQ_TYPE = 0x0C;
        internal const byte MQTT_MSG_PINGRESP_TYPE = 0x0D;
        internal const byte MQTT_MSG_DISCONNECT_TYPE = 0x0E;

        // QOS levels
        public const byte QOS_LEVEL_AT_MOST_ONCE = 0x00;
        public const byte QOS_LEVEL_AT_LEAST_ONCE = 0x01;
        public const byte QOS_LEVEL_EXACTLY_ONCE = 0x02;

        internal const ushort MAX_TOPIC_LENGTH = 65535;
        internal const ushort MIN_TOPIC_LENGTH = 1;
        internal const byte MESSAGE_ID_SIZE = 2;

        #endregion

        #region Properties...

        /// <summary>
        /// Message type
        /// </summary>
        public byte Type
        {
            get { return this.type; }
            set { this.type = value; }
        }

        /// <summary>
        /// Duplicate message flag
        /// </summary>
        public bool DupFlag
        {
            get { return this.dupFlag; }
            set { this.dupFlag = value; }
        }

        /// <summary>
        /// Quality of Service level
        /// </summary>
        public byte QosLevel
        {
            get { return this.qosLevel; }
            set { this.qosLevel = value; }
        }

        /// <summary>
        /// Retain message flag
        /// </summary>
        public bool Retain
        {
            get { return this.retain; }
            set { this.retain = value; }
        }

        #endregion

        // message type
        protected byte type;
        // duplicate delivery
        protected bool dupFlag;
        // quality of service level
        protected byte qosLevel;
        // retain flag
        protected bool retain;

        /// <summary>
        /// Returns message bytes rapresentation
        /// </summary>
        /// <returns>Bytes rapresentation</returns>
        public abstract byte[] GetBytes();
        
        /// <summary>
        /// Encode remaining length and insert it into message buffer
        /// </summary>
        /// <param name="remainingLength">Remaining length value to encode</param>
        /// <param name="buffer">Message buffer for inserting encoded value</param>
        /// <param name="index">Index from which insert encoded value into buffer</param>
        /// <returns>Index updated</returns>
        protected int encodeRemainingLength(int remainingLength, byte[] buffer, int index)
        {
            int digit = 0;
            do
            {
                digit = remainingLength % 128;
                remainingLength /= 128;
                if (remainingLength > 0)
                    digit = digit | 0x80;
                buffer[index++] = (byte)digit;
            } while (remainingLength > 0);
            return index;
        }

        /// <summary>
        /// Decode remaining length reading bytes from socket
        /// </summary>
        /// <param name="channel">Channel from reading bytes</param>
        /// <returns>Decoded remaining length</returns>
        protected static int decodeRemainingLength(IMqttNetworkChannel channel)
        {
            int multiplier = 1;
            int value = 0;
            int digit = 0;
            byte[] nextByte = new byte[1];
            do
            {
                // next digit from stream
                channel.Receive(nextByte);
                digit = nextByte[0];
                value += ((digit & 127) * multiplier);
                multiplier *= 128;
            } while ((digit & 128) != 0);
            return value;
        }

#if TRACE
        /// <summary>
        /// Returns a string representation of the message for tracing
        /// </summary>
        /// <param name="name">Message name</param>
        /// <param name="fieldNames">Message fields name</param>
        /// <param name="fieldValues">Message fields value</param>
        /// <returns>String representation of the message</returns>
        protected string GetTraceString(string name, object[] fieldNames, object[] fieldValues)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(name);
            
            if ((fieldNames != null) && (fieldValues != null))
            {
                sb.Append("(");
                bool addComma = false;
                for (int i = 0; i < fieldValues.Length; i++)
                {
                    if (fieldValues[i] != null)
                    {
                        if (addComma)
                        {
                            sb.Append(",");
                        }

                        sb.Append(fieldNames[i]);
                        sb.Append(":");
                        sb.Append(GetStringObject(fieldValues[i]));
                        addComma = true;
                    }
                }
                sb.Append(")");
            }
            
            return sb.ToString();
        }

        object GetStringObject(object value)
        {
            byte[] binary = value as byte[];
            if (binary != null)
            {
                string hexChars = "0123456789ABCDEF";
                StringBuilder sb = new StringBuilder(binary.Length * 2);
                for (int i = 0; i < binary.Length; ++i)
                {
                    sb.Append(hexChars[binary[i] >> 4]);
                    sb.Append(hexChars[binary[i] & 0x0F]);
                }

                return sb.ToString();
            }

            object[] list = value as object[];
            if (list != null)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append('[');
                for (int i = 0; i < list.Length; ++i)
                {
                    if (i > 0) sb.Append(',');
                    sb.Append(list[i]);
                }
                sb.Append(']');

                return sb.ToString();
            }

            return value;
        }
#endif
    }
}
