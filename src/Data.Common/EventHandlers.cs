﻿namespace DevZest.Data
{
    public delegate void DataRowAddedEventHandler(DataRow dataRow);

    public delegate void DataRowRemovedEventHandler(DataRow dataRow, DataSet baseDataSet, int ordinal, DataSet dataSet, int index);

    public delegate void ValueChangedEventHandler(DataRow dataRow, IColumnSet columns);
}