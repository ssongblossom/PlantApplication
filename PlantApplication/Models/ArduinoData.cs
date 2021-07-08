using System;


namespace PlantApplication.Models
{
    public class ArduinoData
    {

        static string[] strArr = new string[] { "DHT11", "DHT22", "DM460", "DM2007", "CO", "Alcohol", "CO2", "Tolueno", "NH4", "Acetona" };
        enum Sensors { DHT11, DHT22, DM460, DM2007, CO, Alcohol, CO2, Tolueno, NH4, Acetona, NumOfSensors }

        public DateTime Date { get; set; }

        public double DHT11Temp { get; set; }

        public double DHT11Humid { get; set; }

        public double DHT22Temp { get; set; }

        public double DHT22Humid { get; set; }

        public int DM460LightIntensity { get; set; }

        public int DM2007LightIntensity { get; set; }

        public double CO { get; set; }

        public double Alcohol { get; set; }

        public double CO2 { get; set; }

        public double Tolueno { get; set; }

        public double NH4 { get; set; }

        public double Acetona { get; set; }

        public bool IsLastReadSuccessful { get; set; }


        public static void DataProcessor(string recievedData, ArduinoData data)
        {
            string strData = recievedData.Substring(0, recievedData.Length); //전달받은 데이터를 그대로 값복사하여 저장
            int index = 0;
            data.IsLastReadSuccessful = false; //기본값 설정


            //데이터를 ':'를 기준으로 split 0번째에는 센서이름, 1번째에는 센서 값 (ex DM460: 110 -> datas[0] : DM460 , datas[1] : 110
            string[] datas = strData.Split(':');

            //받은 데이터가 어떤 센서의 데이터인지 구하기 
            for (int i = 0; i < (int)Sensors.NumOfSensors; i++)
            {
                string sensorName = strArr[i];
                if (datas[0].Equals(sensorName))
                {
                    index = i;
                    break;
                }
            }

            datas[1].Replace(" ", ""); // 공백 제거

            try
            {
                //센서의 종류에 따라서 데이터 가공하기

                if (index == (int)Sensors.DHT11 || index == (int)Sensors.DHT22)    //dht11 or dht22 : (temp,humid)
                {
                    string[] subs = datas[1].Split(','); //0: '(temp' /1: 'humid)'
                    string temp = subs[0].Remove(0, 1);    // '(' 제거
                    string humid = subs[1].Remove(subs[1].Length - 3); // ')' 제거

                    if (index == (int)Sensors.DHT11)
                    {
                        data.DHT11Temp = Convert.ToDouble(temp);
                        data.DHT11Humid = Convert.ToDouble(humid);
                    }
                    else if (index == (int)Sensors.DHT22)
                    {
                        data.DHT22Temp = Convert.ToDouble(temp);
                        data.DHT22Humid = Convert.ToDouble(humid);
                    }
                }
                //그 외 센서
                else
                {
                    switch (index)
                    {
                        case (int)Sensors.DM460:
                            data.DM460LightIntensity = Convert.ToInt32(datas[1]);
                            break;
                        case (int)Sensors.DM2007:
                            data.DM2007LightIntensity = Convert.ToInt32(datas[1]);
                            break;
                        case (int)Sensors.CO:
                            data.CO = Convert.ToDouble(datas[1]);
                            break;
                        case (int)Sensors.Alcohol:
                            data.Alcohol = Convert.ToDouble(datas[1]);
                            break;
                        case (int)Sensors.CO2:
                            data.CO2 = Convert.ToDouble(datas[1]);
                            break;
                        case (int)Sensors.Tolueno:
                            data.Tolueno = Convert.ToDouble(datas[1]);
                            break;
                        case (int)Sensors.NH4:
                            data.NH4 = Convert.ToDouble(datas[1]);
                            break;
                        case (int)Sensors.Acetona:
                            data.Acetona = Convert.ToDouble(datas[1]);
                            data.IsLastReadSuccessful = true; //마지막 데이터를 받았을 때만 true로 바꾸어주기
                            break;
                        default:
                            break;
                    }
                }
            }catch(Exception e)
            {

            }
        }
    }


}
