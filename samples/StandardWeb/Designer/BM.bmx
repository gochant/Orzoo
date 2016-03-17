<?xml version="1.0"?>
<BusinessModelContainer xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xsi:type="BusinessModelContainerViewModel">
  <EdmFilePath>Designer\ER.edmx</EdmFilePath>
  <SaveFilePath>C:\Users\QingGuo\Documents\Code\DynamicWeb\StandardWeb\Designer\BM.bmx</SaveFilePath>
  <Packages>
    <BusinessModelPackage xsi:type="BusinessModelPackageViewModel" Name="Person" Display="人员">
      <CommonModel xsi:type="CommonModelDefineViewModel">
        <Fields>
          <ModelFieldDefine xsi:type="ModelFieldDefineViewModel" Name="Id" Type="int" IsCalculated="false" IsReadonly="false" IsKey="false">
            <CtoVMap IsUsedAsExpression="true" />
            <VtoCMap IsUsedAsExpression="true" />
            <Customs />
            <Display IsApplicable="true" Enabled="true" Name="Id" IsI18N="false" />
            <DisplayField IsApplicable="true" Enabled="false" />
            <DataType IsApplicable="true" Enabled="false" />
            <DisplayFormat IsApplicable="true" Enabled="false" />
            <UIHint IsApplicable="true" Enabled="true" Hint="Hidden" />
            <Required IsApplicable="true" Enabled="true" AllowEmptyStrings="false" />
            <StringLength IsApplicable="false" Enabled="false" Min="0" Max="50" />
            <Range IsApplicable="true" Enabled="false" Type="int" />
            <RegExp IsApplicable="true" Enabled="false" />
          </ModelFieldDefine>
          <ModelFieldDefine xsi:type="ModelFieldDefineViewModel" Name="Name" Type="string" IsCalculated="false" IsReadonly="false" IsKey="false">
            <CtoVMap IsUsedAsExpression="true" />
            <VtoCMap IsUsedAsExpression="true" />
            <Customs />
            <Display IsApplicable="true" Enabled="true" Name="名称" IsI18N="false" />
            <DisplayField IsApplicable="true" Enabled="false" />
            <DataType IsApplicable="true" Enabled="false" />
            <DisplayFormat IsApplicable="true" Enabled="false" />
            <UIHint IsApplicable="true" Enabled="false" />
            <Required IsApplicable="true" Enabled="true" AllowEmptyStrings="false" />
            <StringLength IsApplicable="true" Enabled="false" Min="0" Max="50" />
            <Range IsApplicable="true" Enabled="false" Type="string" />
            <RegExp IsApplicable="true" Enabled="false" />
          </ModelFieldDefine>
          <ModelFieldDefine xsi:type="ModelFieldDefineViewModel" Name="Email" Type="string" IsCalculated="false" IsReadonly="false" IsKey="false">
            <CtoVMap IsUsedAsExpression="true" />
            <VtoCMap IsUsedAsExpression="true" />
            <Customs />
            <Display IsApplicable="true" Enabled="true" Name="邮件" IsI18N="false" />
            <DisplayField IsApplicable="true" Enabled="false" FieldName="" />
            <DataType IsApplicable="true" Enabled="true" Type="EmailAddress" />
            <DisplayFormat IsApplicable="true" Enabled="false" DataFormatString="" />
            <UIHint IsApplicable="true" Enabled="false" />
            <Required IsApplicable="true" Enabled="false" AllowEmptyStrings="false" />
            <StringLength IsApplicable="true" Enabled="false" Min="0" Max="50" />
            <Range IsApplicable="true" Enabled="false" Type="string" />
            <RegExp IsApplicable="true" Enabled="false" />
          </ModelFieldDefine>
          <ModelFieldDefine xsi:type="ModelFieldDefineViewModel" Name="Birth" Type="Nullable&lt;System.DateTime&gt;" IsCalculated="false" IsReadonly="false" IsKey="false">
            <CtoVMap IsUsedAsExpression="true" />
            <VtoCMap IsUsedAsExpression="true" />
            <Customs />
            <Display IsApplicable="true" Enabled="true" Name="生日" IsI18N="false" />
            <DisplayField IsApplicable="true" Enabled="false" />
            <DataType IsApplicable="true" Enabled="false" />
            <DisplayFormat IsApplicable="true" Enabled="false" />
            <UIHint IsApplicable="true" Enabled="false" />
            <Required IsApplicable="true" Enabled="false" AllowEmptyStrings="false" />
            <StringLength IsApplicable="false" Enabled="false" Min="0" Max="50" />
            <Range IsApplicable="true" Enabled="false" Type="Nullable&lt;System.DateTime&gt;" />
            <RegExp IsApplicable="true" Enabled="false" />
          </ModelFieldDefine>
          <ModelFieldDefine xsi:type="ModelFieldDefineViewModel" Name="Display" Type="string" IsCalculated="true" IsReadonly="false" IsKey="false" AssociatedField="Name">
            <CtoVMap IsUsedAsExpression="true" SourceCode="d.Name + &quot;(&quot; + d.Email  + &quot;}&quot;" />
            <VtoCMap IsUsedAsExpression="true" />
            <Customs />
            <Display IsApplicable="true" Enabled="true" Name="显示" IsI18N="false" />
            <DisplayField IsApplicable="true" Enabled="false" />
            <DataType IsApplicable="true" Enabled="false" />
            <DisplayFormat IsApplicable="true" Enabled="false" />
            <UIHint IsApplicable="true" Enabled="false" />
            <Required IsApplicable="true" Enabled="false" AllowEmptyStrings="false" />
            <StringLength IsApplicable="true" Enabled="false" Min="0" Max="50" />
            <Range IsApplicable="true" Enabled="false" Type="string" />
            <RegExp IsApplicable="true" Enabled="false" />
          </ModelFieldDefine>
        </Fields>
      </CommonModel>
      <ViewModels>
        <ViewModelDefine xsi:type="ViewModelDefineViewModel" Name="PersonDto" DisplayName="人员" EnableReverseMap="true">
          <Fields>
            <ViewModelFieldDefine Name="Id" IsScaffolding="false" />
            <ViewModelFieldDefine Name="Name" IsScaffolding="false" />
            <ViewModelFieldDefine Name="Email" IsScaffolding="false" />
            <ViewModelFieldDefine Name="Birth" IsScaffolding="false" />
          </Fields>
        </ViewModelDefine>
        <ViewModelDefine xsi:type="ViewModelDefineViewModel" Name="PersonPersonItemDto" DisplayName="人员" EnableReverseMap="false">
          <Fields>
            <ViewModelFieldDefine Name="Id" IsScaffolding="false" />
            <ViewModelFieldDefine Name="Name" IsScaffolding="false" />
            <ViewModelFieldDefine Name="Email" IsScaffolding="false" />
            <ViewModelFieldDefine Name="Birth" IsScaffolding="false" />
            <ViewModelFieldDefine Name="Display" IsScaffolding="false" />
          </Fields>
        </ViewModelDefine>
      </ViewModels>
      <CrudUserStory xsi:type="CrudUserStoryViewModel" ListItemViewModel="PersonPersonItemDto" EditViewModel="PersonDto" />
    </BusinessModelPackage>
  </Packages>
</BusinessModelContainer>