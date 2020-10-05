using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RevitParametersCodeGenerator
{
    public static class SharedParametersFile
    {
        /// <summary>
        ///W_Плинтус_Наличие
        ///Помещения/Экземпляр/Аналитическая модель (Параметр проекта)
        /// </summary>
        public static Guid W_Plintus_Nalichie = new Guid("31651f00-7a68-4385-a082-ab5a0a6d5611");
        /// <summary>
        ///Имя помещения
        /// </summary>
        public static Guid Imya_Pomeshcheniya = new Guid("d70e5000-65a9-4807-8abb-5dd92bb29dcf");
        /// <summary>
        ///W_Стены_Износ
        /// </summary>
        public static Guid W_Steny_Iznos = new Guid("c643ea00-1ec6-4904-931e-f74d264ccede");
        /// <summary>
        ///Радиус сферы
        /// </summary>
        public static Guid Radius_Sfery = new Guid("8d514f01-b276-4955-8168-b8b6b0a9ead2");
        /// <summary>
        ///Чистовая з.п.
        /// </summary>
        public static Guid CHistovaya_Zp = new Guid("b06a5a01-8473-402b-8b5f-4a01886218bf");
        /// <summary>
        ///W_Проем_Ширина
        /// </summary>
        public static Guid W_Proem_SHirina = new Guid("85a36601-7a5a-4d99-8879-c820db2742f1");
        /// <summary>
        ///Завод-Изготовитель
        /// </summary>
        public static Guid ZavodIzgotovitel = new Guid("62f16c01-2cc4-460e-bd78-6b85fb3b2bc1");
        /// <summary>
        ///6_Лист
        /// </summary>
        public static Guid List6_ = new Guid("b1cabb01-d175-4579-a35b-9b681a8ad47d");
        /// <summary>
        ///W_Пята_Высота
        /// </summary>
        public static Guid W_Pyata_Vysota = new Guid("6b02c901-6f83-423e-8bc6-b70274c7715e");
        /// <summary>
        ///W_Сечение_Диаметр
        /// </summary>
        public static Guid W_Sechenie_Diametr = new Guid("c05ed601-5b80-48aa-b679-286bf5606633");
        /// <summary>
        ///8_Количество участков
        /// </summary>
        public static Guid Kolichestvo_Uchastkov8_ = new Guid("1b9a3402-48f5-41da-bc0e-126a2736f76a");
        /// <summary>
        ///Единица измерения
        /// </summary>
        public static Guid Edinitsa_Izmereniya = new Guid("3e084002-9168-4d7a-82ae-0efa3e265c44");
        /// <summary>
        ///W_Площадь поверхности на 1т профиля
        /// </summary>
        public static Guid W_Ploshchad_Poverkhnosti_Na_1t_Profilya = new Guid("c96bcb02-505a-4731-b393-8303f76b6c10");
        /// <summary>
        ///Отделка стен (описание)
        /// </summary>
        public static Guid Otdelka_Sten_Opisanie = new Guid("b2ded802-1795-4868-8ccb-8aa33599536e");
        /// <summary>
        ///W_Откосы_Описание
        ///Окна/Экземпляр/Строительство (Параметр семейства)
        /// </summary>
        public static Guid W_Otkosy_Opisanie = new Guid("ab70db02-469e-4f3f-848e-650fde3d418c");
        /// <summary>
        ///Лист
        /// </summary>
        public static Guid List = new Guid("526bf202-067e-41dc-9936-df048bf27d56");
        /// <summary>
        ///W_Внешняя высота откоса на плоскости рамы_(от основания до основания)
        /// </summary>
        public static Guid W_Vneshnyaya_Vysota_Otkosa_Na_Ploskosti_Ramy_Ot_Osnovaniya_Do_Osnovaniya = new Guid("58d81303-3303-44be-9ba9-92dfc2ae21d5");
        /// <summary>
        ///W_Перемычки_высота
        /// </summary>
        public static Guid W_Peremychki_Vysota = new Guid("ffce1f03-a4f0-4306-8ccf-4e5982b4c8e0");
        /// <summary>
        ///Количество в помещении
        /// </summary>
        public static Guid Kolichestvo_V_Pomeshchenii = new Guid("e5fd8f03-0e9e-4e94-83ac-a8a5e7924ed8");
        /// <summary>
        ///Тип пола
        /// </summary>
        public static Guid Tip_Pola = new Guid("bb3fbe03-6b93-46ad-bc7d-8a32953a377f");
        /// <summary>
        ///ГБ отделка
        /// </summary>
        public static Guid GB_Otdelka = new Guid("ca09c803-ba1b-47e5-b881-9fe9855f2947");
        /// <summary>
        ///Высота подключения
        /// </summary>
        public static Guid Vysota_Podklyucheniya = new Guid("898ed003-06af-48d0-acef-b22cd9a2cba5");
        /// <summary>
        ///WSH_Периметр без проемов
        ///Помещения/Экземпляр/Аналитическая модель (Параметр проекта)
        /// </summary>
        public static Guid WSH_Perimetr_Bez_Proemov = new Guid("f05eed03-1b3d-4ce7-950a-5d3951cb67d0");
        /// <summary>
        ///W_Огнестойкость
        ///Двери/Тип/Зависимости (Параметр семейства)
        /// </summary>
        public static Guid W_Ognestoykost = new Guid("378dfa03-869b-41ec-840f-ec0b8cd1c38a");
        /// <summary>
        ///W_Материал
        ///Окна, двери - материал заполнения:&#xD&#xA- деревянный&#xD&#xA- стальной&#xD&#xA- из алюминиевых профилеей&#xD&#xA- и др.
        /// </summary>
        public static Guid W_Material = new Guid("f6984d04-8d13-4ac8-9978-fe4c35392727");
        /// <summary>
        ///W_Двери_Примечание_Ударопрочное_стекло
        ///Ударопрочное стекло по ГОСТ Р 51136-2008&#xD&#xA
        /// </summary>
        public static Guid W_Dveri_Primechanie_Udaroprochnoe_Steklo = new Guid("82c66e04-a2d8-4e45-bdb7-88f1b9ff2165");
        /// <summary>
        ///Изделие №6
        /// </summary>
        public static Guid Izdelie_6 = new Guid("6d201505-6b3f-42cc-8393-084aedd070fe");
        /// <summary>
        ///Комментарии изменений
        /// </summary>
        public static Guid Kommentarii_Izmeneniy = new Guid("57242405-6981-4c6f-9fdc-f2827eb915ac");
        /// <summary>
        ///Шумоизоляция
        /// </summary>
        public static Guid SHumoizolyatsiya = new Guid("888a7805-6b58-410f-ab81-0c252a4cd702");
        /// <summary>
        ///К Исп. устан. мощн. 0.7..0.8
        /// </summary>
        public static Guid K_Isp_Ustan_Moshchn_0708 = new Guid("7910ac05-4fc6-49a4-8393-dda2efee121d");
        /// <summary>
        ///8_Номер изменения
        /// </summary>
        public static Guid Nomer_Izmeneniya8_ = new Guid("c261d105-3a05-46b1-9ebe-2ff5204c9b64");
        /// <summary>
        ///W_Двери_Примечание_Уплотнитель
        ///Уплотнитель в притворе&#xD&#xA
        /// </summary>
        public static Guid W_Dveri_Primechanie_Uplotnitel = new Guid("0eea0f06-495f-451a-b148-a0276075a732");
        /// <summary>
        ///W_Кабина_Ширина
        ///Оборудование/Тип/Размеры (Параметр семейства)
        /// </summary>
        public static Guid W_Kabina_SHirina = new Guid("915a6206-63c4-4713-9926-aacd7a0c1da5");
        /// <summary>
        ///WS_Описание отделки по ЖБ
        ///Помещения/Экземпляр/Аналитическая модель (Параметр проекта)
        /// </summary>
        public static Guid WS_Opisanie_Otdelki_Po_ZHB = new Guid("a677b606-518f-4a02-8dc6-64a672a53de7");
        /// <summary>
        ///Область в осях
        /// </summary>
        public static Guid Oblast_V_Osyakh = new Guid("538ec706-9310-4546-9d7e-95b780cb5b93");
        /// <summary>
        ///W_Помещение
        /// </summary>
        public static Guid W_Pomeshchenie = new Guid("fb7bb107-05a1-40f2-b044-c478bbdf5694");
        /// <summary>
        ///W_Индекс звукоизоляции, дБ
        ///Тип/Текст (Параметр проекта)
        /// </summary>
        public static Guid W_Indeks_Zvukoizolyatsii_DB = new Guid("d880d507-69fd-48cf-bb47-b1cf49545b4b");
        /// <summary>
        ///ГК отделка
        /// </summary>
        public static Guid GK_Otdelka = new Guid("9c50f307-a33f-4a71-a558-f6aff339eb03");
        /// <summary>
        ///Звуковое давление
        ///Звуковое давление на 1 Вт, дБ
        /// </summary>
        public static Guid Zvukovoe_Davlenie = new Guid("03180308-3de7-4ee8-a888-37c9523a882d");
        /// <summary>
        ///Ограничение по уровню (Этаж)
        /// </summary>
        public static Guid Ogranichenie_Po_Urovnyu_Etazh = new Guid("323f1708-e1e0-4b34-865c-ee4e54af75d4");
        /// <summary>
        ///Поставщик
        /// </summary>
        public static Guid Postavshchik = new Guid("35837e08-00ea-40b4-9ff3-eb29c9058717");
        /// <summary>
        ///Глубина щита
        /// </summary>
        public static Guid Glubina_SHCHita = new Guid("8fd98c08-7362-4654-a268-54f38f81528f");
        /// <summary>
        ///Давление
        /// </summary>
        public static Guid Davlenie = new Guid("fce60d09-90d7-4e05-9ab6-8bfeeccdc5d2");
        /// <summary>
        ///2_Дата
        /// </summary>
        public static Guid Data2_ = new Guid("eb787709-ec38-4209-b5d8-9d7a504bfa92");
        /// <summary>
        ///Вес с нагрузкой
        /// </summary>
        public static Guid Ves_S_Nagruzkoy = new Guid("e732d909-0da3-4725-afd8-fc42e59e3079");
        /// <summary>
        ///AGSS
        /// </summary>
        public static Guid AGSS = new Guid("5c92f209-a3b2-464c-8683-db0115200e83");
        /// <summary>
        ///Количество
        /// </summary>
        public static Guid Kolichestvo = new Guid("ffe4130a-47c4-45fb-b095-c349db834ced");
        /// <summary>
        ///W_Отбойная доска_Описание
        ///Помещения/Экземпляр/Аналитическая модель (Параметр проекта)
        /// </summary>
        public static Guid W_Otboynaya_Doska_Opisanie = new Guid("335e600a-965a-4704-9cf8-6e2dd3049e0a");
        /// <summary>
        ///Данные элементов пола
        /// </summary>
        public static Guid Dannye_Elementov_Pola = new Guid("2355870b-540f-42c9-aa78-315b2c8d8df0");
        /// <summary>
        ///Расход/диаметр патрубка воздуха
        /// </summary>
        public static Guid Raskhoddiametr_Patrubka_Vozdukha = new Guid("7da6a30b-f3b7-4b21-a688-bb229fbe6a57");
        /// <summary>
        ///Максимальное рабочее напряжение
        /// </summary>
        public static Guid Maksimalnoe_Rabochee_Napryazhenie = new Guid("6d4ddd0b-86ac-499e-ac6c-e529384afdbf");
        /// <summary>
        ///Артикул
        /// </summary>
        public static Guid Artikul = new Guid("96ae3c0c-0303-45f1-a93b-17a55f52b3e4");
        /// <summary>
        ///Постоянное рабочее место
        /// </summary>
        public static Guid Postoyannoe_Rabochee_Mesto = new Guid("8ff94d0c-02d1-4ba4-9f2a-4cbeda8dcd4b");
        /// <summary>
        ///W_Наименование_Тип
        /// </summary>
        public static Guid W_Naimenovanie_Tip = new Guid("2399a20c-e4c6-40e8-8c64-2253e8306c20");
        /// <summary>
        ///W_Подоконник_Ширина
        ///Окна/Экземпляр/Материалы и отделка (Параметр семейства)
        /// </summary>
        public static Guid W_Podokonnik_SHirina = new Guid("a80d490d-651c-4b56-9ee9-951e6b8fa383");
        /// <summary>
        ///Смещение для электрической цепи
        /// </summary>
        public static Guid Smeshchenie_Dlya_Elektricheskoy_TSepi = new Guid("8598960d-1bdb-4e68-82e0-f5202c76bce1");
        /// <summary>
        ///W_Способ подсчета массы
        /// </summary>
        public static Guid W_Sposob_Podscheta_Massy = new Guid("cdc9100e-0889-419b-9d58-3395ad4e371f");
        /// <summary>
        ///Класс электробезопасности
        /// </summary>
        public static Guid Klass_Elektrobezopasnosti = new Guid("9b4a4d0e-36f2-4339-b4bf-31902eb27580");
        /// <summary>
        ///W_Масса погонного метра изделия
        /// </summary>
        public static Guid W_Massa_Pogonnogo_Metra_Izdeliya = new Guid("046b6c0e-7922-42bf-a8b5-9dce70b8df1e");
        /// <summary>
        ///Сквозная нумерация
        /// </summary>
        public static Guid Skvoznaya_Numeratsiya = new Guid("18bbc10e-1346-4a94-beaa-992e674e81f1");
        /// <summary>
        ///W_Глубина внешнего откоса_(от внешней штукатурки до кирпичного перепада)
        /// </summary>
        public static Guid W_Glubina_Vneshnego_Otkosa_Ot_Vneshney_SHtukaturki_Do_Kirpichnogo_Perepada = new Guid("2f9df90e-fab4-4ca0-8b9a-416a8129eea9");
        /// <summary>
        ///_Температура макс
        /// </summary>
        public static Guid Temperatura_Maks_ = new Guid("33dafc0e-2e3d-4a72-869e-d4834ed3ecc0");
        /// <summary>
        ///Тепловыделение
        /// </summary>
        public static Guid Teplovydelenie = new Guid("70dffc0e-688c-4b37-8911-c8e36bf31c46");
        /// <summary>
        ///Толщина перемычки
        /// </summary>
        public static Guid Tolshchina_Peremychki = new Guid("1b30150f-2468-476a-991d-ad44429552e3");
        /// <summary>
        ///Число
        /// </summary>
        public static Guid CHislo = new Guid("34c15d0f-48ea-4245-b006-a3af1dd66235");
        /// <summary>
        ///9_Номер изменения
        /// </summary>
        public static Guid Nomer_Izmeneniya9_ = new Guid("1130b00f-17fe-4a62-b553-141ec08fd824");
        /// <summary>
        ///Чистая площадь стен
        /// </summary>
        public static Guid CHistaya_Ploshchad_Sten = new Guid("2c71c10f-4304-40c3-bf79-85162c017d7f");
        /// <summary>
        ///W_Двери_Монтажный_проём_высота
        /// </summary>
        public static Guid W_Dveri_Montazhnyy_Proem_Vysota = new Guid("6b0fc80f-0709-4cce-8028-294d0fbe61c5");
        /// <summary>
        ///Маркировка контактора в цепи
        /// </summary>
        public static Guid Markirovka_Kontaktora_V_TSepi = new Guid("b62f5710-5194-4895-9970-e2b110e5511a");
        /// <summary>
        ///W_Двери_Обозначение_Конструкция дверного полотна
        /// </summary>
        public static Guid W_Dveri_Oboznachenie_Konstruktsiya_Dvernogo_Polotna = new Guid("b2276210-3643-4c75-8d05-f1888df9afb9");
        /// <summary>
        ///КЛ - Код изделия
        /// </summary>
        public static Guid KL__Kod_Izdeliya = new Guid("8e746310-59fc-46eb-aaff-f29a528deba6");
        /// <summary>
        ///W_Описание
        ///Двери/Тип/Зависимости (Параметр семейства)
        /// </summary>
        public static Guid W_Opisanie = new Guid("ae7fb110-7ddf-4e45-b0f2-8d9747c73abe");
        /// <summary>
        ///Постоянн./Непостоянные места
        /// </summary>
        public static Guid PostoyannNepostoyannye_Mesta = new Guid("7cd7b810-1c26-43a7-b463-0de468afc822");
        /// <summary>
        ///W_Лестницы_Отделка ступеней и площадок_Описание
        ///Лестницы/Экземпляр/Материалы и отделка (Параметр проекта)
        /// </summary>
        public static Guid W_Lestnitsy_Otdelka_Stupeney_I_Ploshchadok_Opisanie = new Guid("cd06e410-24be-41b8-9ca6-ebeb68f60928");
        /// <summary>
        ///Смещение УГО Y
        /// </summary>
        public static Guid Smeshchenie_UGO_Y = new Guid("a7531a11-0436-44a9-8f2c-d451084c770f");
        /// <summary>
        ///Толщина
        /// </summary>
        public static Guid Tolshchina = new Guid("97ce9611-e630-401c-a0ae-5cd4fe9667c9");
        /// <summary>
        ///Диаметр проёма
        /// </summary>
        public static Guid Diametr_Proema = new Guid("6ebf9911-bb11-4068-80b5-9efa8269a9ed");
        /// <summary>
        ///Фасад освещение
        /// </summary>
        public static Guid Fasad_Osveshchenie = new Guid("baefae11-b40b-4c4e-a66b-c87c09719f2e");
        /// <summary>
        ///W_Откосы_Нар_У стены_Высота дуги
        /// </summary>
        public static Guid W_Otkosy_Nar_U_Steny_Vysota_Dugi = new Guid("d2df5713-10b3-41d9-837c-60b909bdb405");
        /// <summary>
        ///W_Стены_Дефекты
        /// </summary>
        public static Guid W_Steny_Defekty = new Guid("fe418913-f52f-4f28-8b71-dadadfbf7687");
        /// <summary>
        ///01_Объем1
        ///Объем дополнительный
        /// </summary>
        public static Guid Obem101_ = new Guid("cf310314-cb45-4bc1-b3e3-aac93abbf9ab");
        /// <summary>
        ///СВ4
        /// </summary>
        public static Guid SV4 = new Guid("92683914-b61a-41e2-a527-35d8cc0ed83b");
        /// <summary>
        ///Общая длина блока
        /// </summary>
        public static Guid Obshchaya_Dlina_Bloka = new Guid("dbdb7814-813a-4dc3-abec-05c9efeae080");
        /// <summary>
        ///Полные теплопотери
        /// </summary>
        public static Guid Polnye_Teplopoteri = new Guid("7ef5e914-355e-45e6-bc74-0bc815f9fcb5");
        /// <summary>
        ///Тип светильников 2
        /// </summary>
        public static Guid Tip_Svetilnikov_2 = new Guid("813ff414-1c91-422b-a97f-3d1dd0adf2e8");
        /// <summary>
        ///W_Откосы_Вн_У стены_Высота дуги
        /// </summary>
        public static Guid W_Otkosy_Vn_U_Steny_Vysota_Dugi = new Guid("df0af814-034b-4c2e-88d7-a4f4d2481f2a");
        /// <summary>
        ///W_Предмет охраны
        /// </summary>
        public static Guid W_Predmet_Okhrany = new Guid("bbf22415-cff1-43b6-b266-8c8afaac63d6");
        /// <summary>
        ///W_Реставрационные работы_Описание
        /// </summary>
        public static Guid W_Restavratsionnye_Raboty_Opisanie = new Guid("ce72c115-d597-4142-9970-8ec32361527d");
        /// <summary>
        ///Номер откл. ус-ва
        /// </summary>
        public static Guid Nomer_Otkl_Usva = new Guid("c3c10216-e470-42fb-8461-150adfd65751");
        /// <summary>
        ///W_Перемычки_количество_листов
        /// </summary>
        public static Guid W_Peremychki_Kolichestvo_Listov = new Guid("4b1c3d16-c595-4c71-ac3f-5122291a1db3");
        /// <summary>
        ///Временный ID
        /// </summary>
        public static Guid Vremennyy_ID = new Guid("ef775216-48a8-4a6f-b6fd-d0b6e84e7cfb");
        /// <summary>
        ///5_Дата
        /// </summary>
        public static Guid Data5_ = new Guid("99300c17-8719-4344-a1ec-71f592f9e6de");
        /// <summary>
        ///W_Способ открывания
        ///Способ открывания полотна:&#xD&#xA- распашной&#xD&#xA- раздвижной&#xD&#xA- складной&#xD&#xA- качающийся&#xD&#xA- и тд.
        /// </summary>
        public static Guid W_Sposob_Otkryvaniya = new Guid("e0795917-7d55-4f99-93c3-b823f279543a");
        /// <summary>
        ///Высотная отметка
        /// </summary>
        public static Guid Vysotnaya_Otmetka = new Guid("52647417-084f-41f7-9c2d-a3906252df8e");
        /// <summary>
        ///WS_ВОР_Описание_Экз
        ///Стены, оборудование/Экземпляр/Прочее (Параметр проекта)
        /// </summary>
        public static Guid WS_VOR_Opisanie_Ekz = new Guid("f1638417-fe61-43fe-9060-f9738d6cd600");
        /// <summary>
        ///WH_Арматура_Краткое наименование
        ///Экземпляр/Текст (Параметр семейства и проекта)
        /// </summary>
        public static Guid WH_Armatura_Kratkoe_Naimenovanie = new Guid("b179b518-4fca-4550-81fb-966eceeb2b6a");
        /// <summary>
        ///Запретить изменение наименования нагрузки
        /// </summary>
        public static Guid Zapretit_Izmenenie_Naimenovaniya_Nagruzki = new Guid("5de14719-6968-4655-9457-94825e70b623");
        /// <summary>
        ///Курс USD
        /// </summary>
        public static Guid Kurs_USD = new Guid("2f889119-def6-457d-804b-5dd15235546d");
        /// <summary>
        ///10_Номер изменения
        /// </summary>
        public static Guid Nomer_Izmeneniya10_ = new Guid("390ddb19-70cf-47c4-af22-ad7195c36a85");
        /// <summary>
        ///Шахта_Длина
        /// </summary>
        public static Guid SHakhta_Dlina = new Guid("5780fd19-7a2c-46a1-8037-2873e10732b1");
        /// <summary>
        ///Ток_СС_авар
        /// </summary>
        public static Guid Tok_SS_Avar = new Guid("a5094b1a-0375-417b-9a31-e47e3443c53a");
        /// <summary>
        ///W_Дефект_Площадь
        /// </summary>
        public static Guid W_Defekt_Ploshchad = new Guid("ce64a31a-13ee-43d0-8018-6a2a6e9b5566");
        /// <summary>
        ///Врачей
        /// </summary>
        public static Guid Vrachey = new Guid("8cc2d11a-cd25-404c-9159-3429a9ace9e7");
        /// <summary>
        ///W_Высота проёма под короб_(с учётом зазоров)
        /// </summary>
        public static Guid W_Vysota_Proema_Pod_Korob_S_Uchetom_Zazorov = new Guid("4c181e1b-ee91-4af3-b237-5ce2a43a7099");
        /// <summary>
        ///Раздел проектирования
        /// </summary>
        public static Guid Razdel_Proektirovaniya = new Guid("ffe3351b-555f-40fb-86fd-4e5a4a446d27");
        /// <summary>
        ///Напряжение в щите
        /// </summary>
        public static Guid Napryazhenie_V_SHCHite = new Guid("dd0d401b-feb3-4c57-bb19-1e4463b5f310");
        /// <summary>
        ///Сжатый воздух   8 бар
        /// </summary>
        public static Guid Szhatyy_Vozdukh___8_Bar = new Guid("63eec21b-8327-46a8-ac5d-38cc002b3ee7");
        /// <summary>
        ///01_Угол1
        ///Угол. Общий
        /// </summary>
        public static Guid Ugol101_ = new Guid("95f5191c-b612-4834-9c00-0b0fce677847");
        /// <summary>
        ///Интервал крепления к стене (вертикальный лоток)
        /// </summary>
        public static Guid Interval_Krepleniya_K_Stene_Vertikalnyy_Lotok = new Guid("d13c401c-e92d-4465-b6e2-80ea9b43b7bb");
        /// <summary>
        ///W_Стены_Описание отделки
        ///Экземпляр/Строительство (Параметр проекта)
        /// </summary>
        public static Guid W_Steny_Opisanie_Otdelki = new Guid("6607411c-2110-43b3-9a0c-735defb7cea4");
        /// <summary>
        ///Строка 7 фамилия
        /// </summary>
        public static Guid Stroka_7_Familiya = new Guid("d065481c-6992-42e4-933d-4458531eb7d1");
        /// <summary>
        ///W_Откосы_Площадь внутр.
        /// </summary>
        public static Guid W_Otkosy_Ploshchad_Vnutr = new Guid("a47a481c-37e9-4ba6-ba91-f56140550710");
        /// <summary>
        ///W_Сечение_Длина
        /// </summary>
        public static Guid W_Sechenie_Dlina = new Guid("3ac7ac1c-a0ff-4089-80dd-c9165430a8d8");
        /// <summary>
        ///W_Перемычки_ширина
        /// </summary>
        public static Guid W_Peremychki_SHirina = new Guid("9fedb11c-87ac-4891-a132-7ae41a1dddd7");
        /// <summary>
        ///Изменение 5
        /// </summary>
        public static Guid Izmenenie_5 = new Guid("2e27031d-2f6e-431f-849d-d8ee98949694");
        /// <summary>
        ///W_Двери_Обозначение_Распашная/Маятниковая
        /// </summary>
        public static Guid W_Dveri_Oboznachenie_RaspashnayaMayatnikovaya = new Guid("e8f4031d-d924-49d0-b263-dc396fafa504");
        /// <summary>
        ///W_Заполнение_Высота
        /// </summary>
        public static Guid W_Zapolnenie_Vysota = new Guid("2b970f1d-1560-4869-be16-83a4c7209898");
        /// <summary>
        ///N2O
        /// </summary>
        public static Guid N2O = new Guid("8aad341d-ca9e-44d3-a0b1-3935674afbce");
        /// <summary>
        ///W_Класс пожарной безопасноти
        ///Тип/Текст (Параметр проекта)
        /// </summary>
        public static Guid W_Klass_Pozharnoy_Bezopasnoti = new Guid("2cba5e1d-ad80-403a-9b9a-37846573102c");
        /// <summary>
        ///Персонал
        /// </summary>
        public static Guid Personal = new Guid("f63ecb1d-eb04-4863-a9f3-0cfd5cc943ea");
        /// <summary>
        ///К1 отделка
        /// </summary>
        public static Guid K1_Otdelka = new Guid("e2d6161e-6909-4fd1-bf68-51a19f636687");
        /// <summary>
        ///Тепловыделение Ватт
        /// </summary>
        public static Guid Teplovydelenie_Vatt = new Guid("35868f1e-49b1-4264-b85a-99fcd18c9ef5");
        /// <summary>
        ///Длина
        /// </summary>
        public static Guid Dlina = new Guid("7c65e31e-c5f8-4f50-ad09-d346471632b1");
        /// <summary>
        ///W_Двери_Примечание_Нажимная ручка
        /// </summary>
        public static Guid W_Dveri_Primechanie_Nazhimnaya_Ruchka = new Guid("0598ea1e-3963-4006-8cf0-122412435f36");
        /// <summary>
        ///Вредности
        /// </summary>
        public static Guid Vrednosti = new Guid("6888ec1e-7f21-4ba4-833f-47e5987e7cca");
        /// <summary>
        ///W_Наименование
        /// </summary>
        public static Guid W_Naimenovanie = new Guid("9b0e271f-a07c-429c-84f5-2dee205eac3e");
        /// <summary>
        ///WH_Усиление ок.проема_Тип
        ///Тип/Несущие конструкции (Параметр проекта)
        /// </summary>
        public static Guid WH_Usilenie_Okproema_Tip = new Guid("00706d1f-8987-4706-8a27-bf95033e659b");
        /// <summary>
        ///Нормируемая освещенность
        /// </summary>
        public static Guid Normiruemaya_Osveshchennost = new Guid("40e37a1f-ba8e-4c3a-95a2-742040d1ef36");
        /// <summary>
        ///ТФ
        /// </summary>
        public static Guid TF = new Guid("85b8841f-1b7d-4248-a705-dd66c23b1f16");
        /// <summary>
        ///_Тепл. от поступающего воздуха
        /// </summary>
        public static Guid Tepl_Ot_Postupayushchego_Vozdukha_ = new Guid("64cc8e1f-7d5e-486c-9fd3-6a720fdfe9f0");
        /// <summary>
        ///W_Стены_Тип отделки
        /// </summary>
        public static Guid W_Steny_Tip_Otdelki = new Guid("1911e71f-fb26-4750-9933-8ccbb961fc16");
        /// <summary>
        ///2_Номер документа
        /// </summary>
        public static Guid Nomer_Dokumenta2_ = new Guid("24150620-9f8d-4f76-9074-bc6c7956c2d3");
        /// <summary>
        ///WH_Сортировка вида (Тип)
        ///Экземпляр/Набор (Параметр проекта)
        /// </summary>
        public static Guid WH_Sortirovka_Vida_Tip = new Guid("c93e0720-9427-4b8a-aff5-481ecfb1e024");
        /// <summary>
        ///Высота низа проёма
        /// </summary>
        public static Guid Vysota_Niza_Proema = new Guid("790a1520-6866-462a-8610-9eade429f7b7");
        /// <summary>
        ///W_Внутренняя ширина откоса на плоскости рамы_(основание-кирпич)
        /// </summary>
        public static Guid W_Vnutrennyaya_SHirina_Otkosa_Na_Ploskosti_Ramy_Osnovaniekirpich = new Guid("fc365120-7890-4319-bb49-7fb9c2985be6");
        /// <summary>
        ///W_Шахта_Глубина от отм. нижней остановки
        ///Оборудование/Тип/Размеры (Параметр семейства)
        /// </summary>
        public static Guid W_SHakhta_Glubina_Ot_Otm_Nizhney_Ostanovki = new Guid("14eb5321-b2ce-426f-b0b1-fd1d7a02f93a");
        /// <summary>
        ///Раб освещение
        /// </summary>
        public static Guid Rab_Osveshchenie = new Guid("3c74ae21-2862-41a8-9361-e4737cb6e34a");
        /// <summary>
        ///Сквозная нумерация_Видимость
        /// </summary>
        public static Guid Skvoznaya_Numeratsiya_Vidimost = new Guid("0b51b021-ecc8-44d8-9f63-74fd20ac97d2");
        /// <summary>
        ///5_Количество участков
        /// </summary>
        public static Guid Kolichestvo_Uchastkov5_ = new Guid("46e96522-674a-4e13-866e-27be5cffd3c5");
        /// <summary>
        ///Изменение 8
        /// </summary>
        public static Guid Izmenenie_8 = new Guid("f273a122-9860-40c1-b65a-d4a16beab3e3");
        /// <summary>
        ///Не включать в спецификацию
        /// </summary>
        public static Guid Ne_Vklyuchat_V_Spetsifikatsiyu = new Guid("11f28e23-fffe-4bd1-97dc-cb875708fae1");
        /// <summary>
        ///Требования шума
        /// </summary>
        public static Guid Trebovaniya_SHuma = new Guid("a3dc9523-da4d-4c20-a14f-fe92f4a3e626");
        /// <summary>
        ///W_Наличники_Длина
        /// </summary>
        public static Guid W_Nalichniki_Dlina = new Guid("652e4a24-1cf0-4337-b236-e8d21b0b2bf8");
        /// <summary>
        ///Питание от в щитах
        /// </summary>
        public static Guid Pitanie_Ot_V_SHCHitakh = new Guid("8922d624-8769-4848-9487-90cc9156ce9b");
        /// <summary>
        ///W_Верхний монтажный зазор
        /// </summary>
        public static Guid W_Verkhniy_Montazhnyy_Zazor = new Guid("937e0225-b61f-4dce-abbf-6bec1d4ae0a8");
        /// <summary>
        ///W_Измерение_Число
        /// </summary>
        public static Guid W_Izmerenie_CHislo = new Guid("6c9b7a25-75d1-4389-98b5-371806d38ab3");
        /// <summary>
        ///Объём горючей массы
        /// </summary>
        public static Guid Obem_Goryuchey_Massy = new Guid("8a469325-b259-4efc-8402-3e691d4bb255");
        /// <summary>
        ///Группа помещений
        /// </summary>
        public static Guid Gruppa_Pomeshcheniy = new Guid("db1cbe25-9c5c-47e9-8815-d67b8947894a");
        /// <summary>
        ///Строка 3 фамилия
        /// </summary>
        public static Guid Stroka_3_Familiya = new Guid("15284426-f3d8-4c55-9a6c-f9eea1393db3");
        /// <summary>
        ///Метка оси
        /// </summary>
        public static Guid Metka_Osi = new Guid("30939126-af9e-4fdb-82c7-a4c169744f0e");
        /// <summary>
        ///W_Пол_Тип
        ///Экземпляр/Строительство (Параметр проекта)
        /// </summary>
        public static Guid W_Pol_Tip = new Guid("4ad34527-41ed-4f09-b216-48548e5cf46b");
        /// <summary>
        ///W_Окна_Глубина вставки
        /// </summary>
        public static Guid W_Okna_Glubina_Vstavki = new Guid("92704927-33fe-4591-abb4-36f92a102dc8");
        /// <summary>
        ///WH_Правая
        ///Двери/Тип/Зависимости (Параметр семейства)
        /// </summary>
        public static Guid WH_Pravaya = new Guid("a5017927-f818-446d-a8eb-5eedfadbdd50");
        /// <summary>
        ///W_Двери_Примечание_Тип_стеклопакета
        /// </summary>
        public static Guid W_Dveri_Primechanie_Tip_Steklopaketa = new Guid("dc198627-2ba1-412a-ad70-3d971fac6ecb");
        /// <summary>
        ///10_Номер документа
        /// </summary>
        public static Guid Nomer_Dokumenta10_ = new Guid("1217eb27-e5f1-4c43-bcb2-f0a988d996fc");
        /// <summary>
        ///W_Перемычки_уголок_l
        /// </summary>
        public static Guid W_Peremychki_Ugolok_l = new Guid("ac1a7b28-8771-47cc-8c58-5d4d5a103829");
        /// <summary>
        ///W_Добор_Наличие
        /// </summary>
        public static Guid W_Dobor_Nalichie = new Guid("12f18528-d957-44b0-8ad7-a194b628b316");
        /// <summary>
        ///9_Дата
        /// </summary>
        public static Guid Data9_ = new Guid("f0e29928-f12a-4f32-8417-145acdc173eb");
        /// <summary>
        ///_Тепл. от людей
        /// </summary>
        public static Guid Tepl_Ot_Lyudey_ = new Guid("2bb8f028-d3b7-4416-82d9-9ea69546954d");
        /// <summary>
        ///Ширина
        /// </summary>
        public static Guid SHirina = new Guid("2363f328-1d05-4b33-bb06-6607700cb282");
        /// <summary>
        ///Требования освещенности
        /// </summary>
        public static Guid Trebovaniya_Osveshchennosti = new Guid("bdf5f328-3da9-4ce4-a313-c43695486c99");
        /// <summary>
        ///Задание МГ
        /// </summary>
        public static Guid Zadanie_MG = new Guid("df91fd28-cc77-412a-ba6c-daa54af8e043");
        /// <summary>
        ///Масса единицы
        /// </summary>
        public static Guid Massa_Edinitsy = new Guid("e2d5fd28-23a8-46da-bfef-5f199df31a36");
        /// <summary>
        ///Заземляющий проводник
        /// </summary>
        public static Guid Zazemlyayushchiy_Provodnik = new Guid("c7340629-6019-4317-915b-8627881872df");
        /// <summary>
        ///Обозначение кабеля в КЖ
        /// </summary>
        public static Guid Oboznachenie_Kabelya_V_KZH = new Guid("f80e8929-03da-4bfa-b6c9-c70c62e71d49");
        /// <summary>
        ///Класс безопасности
        /// </summary>
        public static Guid Klass_Bezopasnosti = new Guid("b3f6af29-a36d-4c0c-8680-108e40ee8f89");
        /// <summary>
        ///W_Откосы_Нар_У стены_Ширина
        /// </summary>
        public static Guid W_Otkosy_Nar_U_Steny_SHirina = new Guid("04d0d729-5d6a-4c6e-9a8a-16251aab3dd5");
        /// <summary>
        ///W_Длина добора
        /// </summary>
        public static Guid W_Dlina_Dobora = new Guid("4f8a642a-7b2f-4f40-ac8c-f674dd22efc2");
        /// <summary>
        ///Сквозная нумерация double
        /// </summary>
        public static Guid Skvoznaya_Numeratsiya_double = new Guid("88cb0f2b-89b8-4158-8f56-eb605da286c6");
        /// <summary>
        ///6_Количество участков
        /// </summary>
        public static Guid Kolichestvo_Uchastkov6_ = new Guid("76591c2b-5738-4a1c-8eca-c92f39a4c8fc");
        /// <summary>
        ///Количество ламп 2
        /// </summary>
        public static Guid Kolichestvo_Lamp_2 = new Guid("a7c75b2b-1802-45e3-a8bb-e19b0ea7d735");
        /// <summary>
        ///W_Степень огнестойкости
        ///Тип/Текст (Параметр проекта)
        /// </summary>
        public static Guid W_Stepen_Ognestoykosti = new Guid("31529d2b-bb36-4f87-bc95-0e2dd6d317a3");
        /// <summary>
        ///W_Двери_Примечание_Автоматическая
        /// </summary>
        public static Guid W_Dveri_Primechanie_Avtomaticheskaya = new Guid("9d77032c-e4d4-4403-8e39-9411467a58c0");
        /// <summary>
        ///3_Номер документа
        /// </summary>
        public static Guid Nomer_Dokumenta3_ = new Guid("8d68152c-babb-4e6d-acf1-ecd98af12ed9");
        /// <summary>
        ///tg F
        /// </summary>
        public static Guid tg_F = new Guid("e3af772c-03bf-424b-bd53-6ce752352c12");
        /// <summary>
        ///W_Потолок_Черновая отделка
        /// </summary>
        public static Guid W_Potolok_CHernovaya_Otdelka = new Guid("dd05a32c-b698-488c-a7b7-a96aab519187");
        /// <summary>
        ///W_Откосы_Вн_У стены_Высота до дуги
        /// </summary>
        public static Guid W_Otkosy_Vn_U_Steny_Vysota_Do_Dugi = new Guid("64bee12d-41d7-4ea4-95bd-9b4ea5e9fa68");
        /// <summary>
        ///W_Оборудование_Длина
        ///Оборудование/Экземпляр/Размеры (Параметр семейства)
        /// </summary>
        public static Guid W_Oborudovanie_Dlina = new Guid("bbe8322e-0428-49b6-b929-a3d31c18f09e");
        /// <summary>
        ///Функциональное отделение
        /// </summary>
        public static Guid Funktsionalnoe_Otdelenie = new Guid("a75a772e-533e-4271-9d73-9c4a8232f577");
        /// <summary>
        ///WS_Количество компьютеров
        ///Помещения/Экземпляр/Прочее (Параметр проекта)
        /// </summary>
        public static Guid WS_Kolichestvo_Kompyuterov = new Guid("a0a1792e-de8d-4f67-b24b-28e6eb1ca678");
        /// <summary>
        ///Высота перемычки
        /// </summary>
        public static Guid Vysota_Peremychki = new Guid("db2cab2e-f047-490d-9b44-389ef266ce6b");
        /// <summary>
        ///ID кабельного лотка
        /// </summary>
        public static Guid ID_Kabelnogo_Lotka = new Guid("bbd4e42e-9078-4701-91d2-5357de68fefd");
        /// <summary>
        ///Местные отсосы
        /// </summary>
        public static Guid Mestnye_Otsosy = new Guid("f7c4e62e-507d-4952-89e2-3ace91d33155");
        /// <summary>
        ///_Перепад давления
        /// </summary>
        public static Guid Perepad_Davleniya_ = new Guid("3336f92e-b449-480e-b427-b05c5527c4ec");
        /// <summary>
        ///Полезный объем
        /// </summary>
        public static Guid Poleznyy_Obem = new Guid("d2a0442f-20de-405e-b86f-ce6871c53d1d");
        /// <summary>
        ///Оборудование ТХ
        /// </summary>
        public static Guid Oborudovanie_TKH = new Guid("1947462f-8cd7-4dee-a85c-64b36b0787c3");
        /// <summary>
        ///Ток_СС_деж
        /// </summary>
        public static Guid Tok_SS_Dezh = new Guid("2f36652f-6cf4-48e2-85c1-2c50a8b4ee1b");
        /// <summary>
        ///W_Перемычки_масса_лист
        /// </summary>
        public static Guid W_Peremychki_Massa_List = new Guid("e1fbae2f-7d83-456d-ae17-602b4c0eb800");
        /// <summary>
        ///W_Двери_Полотно_Глубина
        /// </summary>
        public static Guid W_Dveri_Polotno_Glubina = new Guid("38f5c92f-9bd8-42a5-8441-5f938b44171f");
        /// <summary>
        ///ОВ_Приток
        /// </summary>
        public static Guid OV_Pritok = new Guid("ffa30630-8183-44b6-9153-fb7faa89f35e");
        /// <summary>
        ///Индекс материала / покрытия
        /// </summary>
        public static Guid Indeks_Materiala__Pokrytiya = new Guid("c52d8930-da7c-478a-8c94-d3c7b7cd36aa");
        /// <summary>
        ///Примечание
        /// </summary>
        public static Guid Primechanie = new Guid("69b48930-ac2a-4f8d-b872-661c78b8f2ca");
        /// <summary>
        ///Тип светильников
        /// </summary>
        public static Guid Tip_Svetilnikov = new Guid("bfeb4131-21e4-4ebf-a21c-f3b5035ad738");
        /// <summary>
        ///Длина через все элементы
        ///Длина цепи как сумма длин от одного приемника до следующего ближайшего
        /// </summary>
        public static Guid Dlina_CHerez_Vse_Elementy = new Guid("43387d31-d9b2-4374-916d-69ce7cec588f");
        /// <summary>
        ///Длительность пребывания
        /// </summary>
        public static Guid Dlitelnost_Prebyvaniya = new Guid("209b9931-3365-44e8-95fc-413f23604e14");
        /// <summary>
        ///W_Группа модели
        /// </summary>
        public static Guid W_Gruppa_Modeli = new Guid("a998b131-1729-4031-87c9-3f5a96399347");
        /// <summary>
        ///WS_ВОР_Единица измерения_Экз
        ///Помещения, стены, оборудование/Экземпляр/Прочее (Параметры проекта)
        /// </summary>
        public static Guid WS_VOR_Edinitsa_Izmereniya_Ekz = new Guid("2b094b32-1971-476c-a23f-b6d0f19f3081");
        /// <summary>
        ///W_Ширина добора
        /// </summary>
        public static Guid W_SHirina_Dobora = new Guid("4bab8c32-6d56-4f72-af39-fbfef702fc78");
        /// <summary>
        ///W_Механ. Замок
        ///Двери/Тип/Зависимости (Параметр семейства)
        /// </summary>
        public static Guid W_Mekhan_Zamok = new Guid("90e09632-0f9b-4210-b8d3-1f46c142967e");
        /// <summary>
        ///Закись азота
        /// </summary>
        public static Guid Zakis_Azota = new Guid("e38ab532-c622-483b-81d3-ebc0be3b6881");
        /// <summary>
        ///W_Марка_Левая
        /// </summary>
        public static Guid W_Marka_Levaya = new Guid("314c2a33-f386-4326-a246-8ad8831efc7b");
        /// <summary>
        ///Eng.Department
        /// </summary>
        public static Guid EngDepartment = new Guid("28e08033-352b-4b14-8105-81e2820ad6e1");
        /// <summary>
        ///W_Внутренняя высота откоса на плоскости рамы_(от основания до основания)
        /// </summary>
        public static Guid W_Vnutrennyaya_Vysota_Otkosa_Na_Ploskosti_Ramy_Ot_Osnovaniya_Do_Osnovaniya = new Guid("17b28933-bfb8-4a0e-8e14-360b50fbbe37");
        /// <summary>
        ///Кислород
        /// </summary>
        public static Guid Kislorod = new Guid("80398d33-0e52-4811-889d-fb13704e85cf");
        /// <summary>
        ///01_Длина
        ///Длина
        /// </summary>
        public static Guid Dlina01_ = new Guid("99eb9d33-caf9-45ee-82d9-49906a913a9e");
        /// <summary>
        ///Код оборудования
        /// </summary>
        public static Guid Kod_Oborudovaniya = new Guid("d9a5fc33-3e6f-4e1b-baf8-aa7ee8143203");
        /// <summary>
        ///Температура воздуха
        /// </summary>
        public static Guid Temperatura_Vozdukha = new Guid("029f0f34-8ec4-45d7-93da-4b99245e3566");
        /// <summary>
        ///W_Фартук_Описание
        ///Помещение/Экземпляр/Строительство (Параметр проекта)
        /// </summary>
        public static Guid W_Fartuk_Opisanie = new Guid("321a4634-af2d-45d7-89ea-ded0f3ffecdd");
        /// <summary>
        ///ID семейства аксессуаров
        /// </summary>
        public static Guid ID_Semeystva_Aksessuarov = new Guid("8ab96534-c015-4dac-912a-de90b116e26f");
        /// <summary>
        ///КЛ - Ед. изм.
        /// </summary>
        public static Guid KL__Ed_Izm = new Guid("6b5d7334-97b3-4259-9e10-edf5047b2573");
        /// <summary>
        ///Класс безопасности медицинских помещений по ГОСТ Р 50571.28-2006
        /// </summary>
        public static Guid Klass_Bezopasnosti_Meditsinskikh_Pomeshcheniy_Po_GOST_R_50571282006 = new Guid("834a7d34-520b-4484-9d50-ceaef8423b2f");
        /// <summary>
        ///W_Наличники_Кол-во сторон
        ///0 - нет&#xD&#xA1 - с одной стороны&#xD&#xA2 - с двух сторон
        /// </summary>
        public static Guid W_Nalichniki_Kolvo_Storon = new Guid("74ac8b34-da0e-4c98-ab74-49492fbc2dc0");
        /// <summary>
        ///Вариант
        /// </summary>
        public static Guid Variant = new Guid("d3a2dc34-4501-4db5-ad77-520d016a93ac");
        /// <summary>
        ///Префикс прибора
        /// </summary>
        public static Guid Prefiks_Pribora = new Guid("a3dff934-1347-406a-b113-326aebaf0e0f");
        /// <summary>
        ///W_Функциональное назначение
        ///Помещение/Экземпляр/Текст (Параметр проекта)
        /// </summary>
        public static Guid W_Funktsionalnoe_Naznachenie = new Guid("2b263d35-a7d6-4b6d-8bce-397be3e68aa2");
        /// <summary>
        ///W_Двери_Примечание_Механический замок
        /// </summary>
        public static Guid W_Dveri_Primechanie_Mekhanicheskiy_Zamok = new Guid("6c26a835-91f7-4e14-8127-0a3273225e5e");
        /// <summary>
        ///W_Тип полотна
        ///Дверной блок (описание полотна):&#xD&#xA- глухой&#xD&#xA- глухой филенчатый&#xD&#xA- остекленный&#xD&#xA- комбинированный&#xD&#xA- др.
        /// </summary>
        public static Guid W_Tip_Polotna = new Guid("c10ae435-9a46-4ba6-93d3-8d839509fb98");
        /// <summary>
        ///Минимальная ширина
        /// </summary>
        public static Guid Minimalnaya_SHirina = new Guid("32af0236-6620-426f-80d6-a96dd313fe4a");
        /// <summary>
        ///Строка 2 фамилия
        /// </summary>
        public static Guid Stroka_2_Familiya = new Guid("6ca82d36-9c55-450a-a479-002c4736cc06");
        /// <summary>
        ///_Тепл. от освещения
        /// </summary>
        public static Guid Tepl_Ot_Osveshcheniya_ = new Guid("85b5c636-98fb-4d88-aa6f-f9a0345bfd16");
        /// <summary>
        ///W_Кабина_Глубина
        ///Оборудование/Тип/Размеры (Параметр семейства)
        /// </summary>
        public static Guid W_Kabina_Glubina = new Guid("0f53fc36-10f9-468a-a621-8ee408f6fb78");
        /// <summary>
        ///Потолок_Высота
        /// </summary>
        public static Guid Potolok_Vysota = new Guid("cf804737-4707-427d-8cec-a42990ab7b1f");
        /// <summary>
        ///W_Двери_Обозначение_Однопольная/Двупольная
        /// </summary>
        public static Guid W_Dveri_Oboznachenie_OdnopolnayaDvupolnaya = new Guid("607a6437-dc37-4621-8732-0b378592e7f9");
        /// <summary>
        ///Пожарная секция
        /// </summary>
        public static Guid Pozharnaya_Sektsiya = new Guid("4c3c8238-86d2-4b53-b042-be1336e19a9b");
        /// <summary>
        ///W_Открывание
        ///Двери/Окна.Тип/Зависимости (Параметр семейства)
        /// </summary>
        public static Guid W_Otkryvanie = new Guid("42e2d638-6760-4eb6-83e7-2fde62fef673");
        /// <summary>
        ///WS_Стены_Площадь чистовой отделки
        ///Экземпляр/Строительство (Параметр проекта)
        /// </summary>
        public static Guid WS_Steny_Ploshchad_CHistovoy_Otdelki = new Guid("0e737d39-d848-4ac2-b1f6-6f60b4a996d0");
        /// <summary>
        ///Строка 1_Дата_Видимость
        /// </summary>
        public static Guid Stroka_1_Data_Vidimost = new Guid("ff98a439-c091-4d62-836e-1c82335479b3");
        /// <summary>
        ///W_Ограждения_Вес м.п.
        ///Ограждения/Тип/Строительство (Параметр проекта)
        /// </summary>
        public static Guid W_Ograzhdeniya_Ves_Mp = new Guid("e477d039-ead3-4c17-876a-ea3e433d0bcd");
        /// <summary>
        ///W_Фахверк_Толщина стены
        ///Обобщенные модели/Тип/Размеры (Параметр семейства) 
        /// </summary>
        public static Guid W_Fakhverk_Tolshchina_Steny = new Guid("0345203a-7520-409d-bffd-d1ba80a53bc2");
        /// <summary>
        ///W_Боковой монтажный зазор
        /// </summary>
        public static Guid W_Bokovoy_Montazhnyy_Zazor = new Guid("efae533a-1159-482e-b8d3-4cea8d8f505b");
        /// <summary>
        ///W_Откосы_Вн_У заполнения_Высота дуги
        /// </summary>
        public static Guid W_Otkosy_Vn_U_Zapolneniya_Vysota_Dugi = new Guid("3aa1ba3a-e7ec-463f-861d-0403f587f496");
        /// <summary>
        ///W_Вес конструкции
        ///Обобщенные модели/Тип/Строительство (Параметр проекта/семейства)
        /// </summary>
        public static Guid W_Ves_Konstruktsii = new Guid("1e32c53a-47f6-48bf-b123-18eb30555b50");
        /// <summary>
        ///W_Светопрозрачное заполнение_Описание
        ///Окна/Тип/Материалы и отделка (Параметр семейства)
        /// </summary>
        public static Guid W_Svetoprozrachnoe_Zapolnenie_Opisanie = new Guid("33021c3b-6441-4269-bdc3-403daa770ec4");
        /// <summary>
        ///W_Добор_Описание
        /// </summary>
        public static Guid W_Dobor_Opisanie = new Guid("1963983b-4a35-47ce-a716-55ed79db8748");
        /// <summary>
        ///W_Кол-во створок
        ///Дверной блок (описание створок):&#xD&#xA- одностворчатый&#xD&#xA- полуторный&#xD&#xA- двустворчатый&#xD&#xA- двустворчатый с боковой фрамугой&#xD&#xA- и др.
        /// </summary>
        public static Guid W_Kolvo_Stvorok = new Guid("02e3e63b-de68-4daf-895e-ac4bc7ddb89a");
        /// <summary>
        ///W_Пол_Отделка
        ///Экземпляр/Строительство (Параметр проекта)
        /// </summary>
        public static Guid W_Pol_Otdelka = new Guid("dfe4f63b-42f2-465c-9e89-59206613cc18");
        /// <summary>
        ///КЛ - Наименование
        /// </summary>
        public static Guid KL__Naimenovanie = new Guid("ae20623c-1df0-4a11-9043-da10b22a3a13");
        /// <summary>
        ///_Тепл. суммарные
        /// </summary>
        public static Guid Tepl_Summarnye_ = new Guid("2053c23c-b535-4f7c-9be1-982051ea5bdf");
        /// <summary>
        ///Уровень1
        /// </summary>
        public static Guid Uroven1 = new Guid("e243533d-e230-4282-86f7-5e793ac3d9ee");
        /// <summary>
        ///WS_ВОР_Описание_Тип
        ///Все категории/Тип/Прочее (Параметр проекта)
        /// </summary>
        public static Guid WS_VOR_Opisanie_Tip = new Guid("cf4a683d-79bb-4401-a924-a2531de82ec8");
        /// <summary>
        ///W_ЭлемФасада_Марка
        /// </summary>
        public static Guid W_ElemFasada_Marka = new Guid("3e0a6d3d-ee9c-450a-8d04-23efe9732709");
        /// <summary>
        ///ГК площадь з.п.
        /// </summary>
        public static Guid GK_Ploshchad_Zp = new Guid("f75e8c3d-cebf-4008-b31f-cdfbfaa23f3c");
        /// <summary>
        ///ГБ площадь з.п.
        /// </summary>
        public static Guid GB_Ploshchad_Zp = new Guid("893b9c3d-3f79-43fd-a6e6-22a4b78477e8");
        /// <summary>
        ///W_Двери_Примечание_Доводчик
        /// </summary>
        public static Guid W_Dveri_Primechanie_Dovodchik = new Guid("24cdd73d-b8e8-4627-87fb-cbd9d6343195");
        /// <summary>
        ///Строка 1 должность
        /// </summary>
        public static Guid Stroka_1_Dolzhnost = new Guid("7a9f273e-9664-4950-9bca-3d96b2670ba6");
        /// <summary>
        ///Телефонная розетка
        /// </summary>
        public static Guid Telefonnaya_Rozetka = new Guid("f664433e-6bc4-4102-b2ff-68256ce360ea");
        /// <summary>
        ///W_Сечение_Ширина
        /// </summary>
        public static Guid W_Sechenie_SHirina = new Guid("0eb64a3e-1865-43d4-9e40-08057e89ad0b");
        /// <summary>
        ///Код материала / покрытия
        /// </summary>
        public static Guid Kod_Materiala__Pokrytiya = new Guid("4e3b273f-c108-46eb-ba80-150caf28ffe2");
        /// <summary>
        ///Стадия на листе
        /// </summary>
        public static Guid Stadiya_Na_Liste = new Guid("7ea3cc3f-2f1d-409e-8414-2b84824f401f");
        /// <summary>
        ///W_Двери_Обозначение_Открывание
        ///Правая/Левая/Равносторонняя&#xD&#xA
        /// </summary>
        public static Guid W_Dveri_Oboznachenie_Otkryvanie = new Guid("7879de3f-1838-4d4e-a2da-b8bb42fb3c01");
        /// <summary>
        ///Медицинский персонал администрации
        /// </summary>
        public static Guid Meditsinskiy_Personal_Administratsii = new Guid("05c0eb3f-b6c0-4c0c-903b-bd965c097100");
        /// <summary>
        ///Корпус
        /// </summary>
        public static Guid Korpus = new Guid("6cc04640-d034-4320-8a02-bdcc774233fa");
        /// <summary>
        ///Строка 6 фамилия
        /// </summary>
        public static Guid Stroka_6_Familiya = new Guid("00178540-addd-412e-83e5-76db04dbb325");
        /// <summary>
        ///Вакуум
        /// </summary>
        public static Guid Vakuum = new Guid("23ac0d42-27cb-45e6-9baf-61b50b23d54c");
        /// <summary>
        ///W_Двери_Примечание_Глухое/Остеклённое
        /// </summary>
        public static Guid W_Dveri_Primechanie_GlukhoeOsteklennoe = new Guid("31f60a43-236c-44fc-8a3a-06be10bf3066");
        /// <summary>
        ///W_Двери_Примечание_Ширина основной створки
        /// </summary>
        public static Guid W_Dveri_Primechanie_SHirina_Osnovnoy_Stvorki = new Guid("ff181a43-1be7-4db5-95cf-122d33021541");
        /// <summary>
        ///Толщина пола
        /// </summary>
        public static Guid Tolshchina_Pola = new Guid("6e932143-e0d5-46e5-a935-71253bf4427f");
        /// <summary>
        ///W_Пол_Схема конструкции
        ///Помещения/Экземпляр/Строительство (Параметр проекта)
        /// </summary>
        public static Guid W_Pol_Skhema_Konstruktsii = new Guid("0f698043-cfae-4055-8946-42122f788f61");
        /// <summary>
        ///Длина кабелей для ОС
        /// </summary>
        public static Guid Dlina_Kabeley_Dlya_OS = new Guid("387ba243-768e-45cf-9c22-ce1b5650fe3d");
        /// <summary>
        ///_Кратность вытяжки
        /// </summary>
        public static Guid Kratnost_Vytyazhki_ = new Guid("3654f443-a132-4857-b1e4-ec6edc22ec3c");
        /// <summary>
        ///Фартук площадь
        /// </summary>
        public static Guid Fartuk_Ploshchad = new Guid("b5620644-273e-4d56-9588-0bbbda77c3fb");
        /// <summary>
        ///W_Мтрл_Материал
        /// </summary>
        public static Guid W_Mtrl_Material = new Guid("ed70ed44-b2a1-4eb3-a84a-3f7f932303fd");
        /// <summary>
        ///Расход на 1 цикл
        /// </summary>
        public static Guid Raskhod_Na_1_TSikl = new Guid("0d5f1e45-56c9-4534-9904-fdee6533a5d1");
        /// <summary>
        ///Спецтребования к проемам
        /// </summary>
        public static Guid Spetstrebovaniya_K_Proemam = new Guid("9f9f5b45-744c-4eb6-8586-fb91c54051f2");
        /// <summary>
        ///Номер листа вручную
        /// </summary>
        public static Guid Nomer_Lista_Vruchnuyu = new Guid("c7687445-c508-4eb1-aefd-3ae0c9e6abfa");
        /// <summary>
        ///Завод-изготовитель
        /// </summary>
        public static Guid Zavodizgotovitel = new Guid("8d467745-7ff7-4363-9935-6f5110f4a8a3");
        /// <summary>
        ///Донная вставка
        /// </summary>
        public static Guid Donnaya_Vstavka = new Guid("74d37745-1c6b-4577-8c94-6e33bb03f1ee");
        /// <summary>
        ///Глубина канала (отчёт)
        /// </summary>
        public static Guid Glubina_Kanala_Otchet = new Guid("3033a745-ef94-43e6-894c-a8b0a9b83477");
        /// <summary>
        ///Позиция в спецификации
        /// </summary>
        public static Guid Pozitsiya_V_Spetsifikatsii = new Guid("e3addd45-825a-411d-9b97-0cf8aef2dc27");
        /// <summary>
        ///W_Добор_Ширина
        /// </summary>
        public static Guid W_Dobor_SHirina = new Guid("9e4c0b46-dafc-4fb6-a62e-cf0c6ca2378c");
        /// <summary>
        ///W_Потолок_Износ
        /// </summary>
        public static Guid W_Potolok_Iznos = new Guid("cf592f46-851e-427a-96a2-57726c0273c2");
        /// <summary>
        ///Элементы пола
        /// </summary>
        public static Guid Elementy_Pola = new Guid("19b48746-ac79-49f0-97e8-f2a4b22de09c");
        /// <summary>
        ///Сопротивление теплопередаче
        /// </summary>
        public static Guid Soprotivlenie_Teploperedache = new Guid("084fca46-fb68-4fc2-9486-d29de50d4520");
        /// <summary>
        ///Мощность подключения
        /// </summary>
        public static Guid Moshchnost_Podklyucheniya = new Guid("1d40f046-7a31-489c-9eda-65ab1e173273");
        /// <summary>
        ///Тепловыделение от освещения
        /// </summary>
        public static Guid Teplovydelenie_Ot_Osveshcheniya = new Guid("8bddf546-bbe9-48c0-98c0-3feb32fb2c2c");
        /// <summary>
        ///Габариты (ШxДxВ)
        /// </summary>
        public static Guid Gabarity_SHxDxV = new Guid("f37b4147-edf3-419e-aad6-da8dc2a392bc");
        /// <summary>
        ///W_Откосы_Отделка
        ///Экземпляр/Строительство (Параметр проекта)
        /// </summary>
        public static Guid W_Otkosy_Otdelka = new Guid("92754847-a837-406b-8707-34384ad2ac30");
        /// <summary>
        ///Строка 6 должность
        /// </summary>
        public static Guid Stroka_6_Dolzhnost = new Guid("81f59847-bc5e-41a2-89f6-f840eaeb0258");
        /// <summary>
        ///Задание ВК
        /// </summary>
        public static Guid Zadanie_VK = new Guid("b2a8c147-064c-46c5-8404-0e7a76351a99");
        /// <summary>
        ///Способ подключения
        /// </summary>
        public static Guid Sposob_Podklyucheniya = new Guid("3265c447-07c8-4e56-9d94-97e8ea14b696");
        /// <summary>
        ///_Тепл. от инсоляции
        /// </summary>
        public static Guid Tepl_Ot_Insolyatsii_ = new Guid("ec6bf647-e0e1-4f88-9731-0690ea48d0d8");
        /// <summary>
        ///W_Откосы_Вн_У заполнения_Высота до дуги
        /// </summary>
        public static Guid W_Otkosy_Vn_U_Zapolneniya_Vysota_Do_Dugi = new Guid("a9963e48-7266-40a7-85ce-7b46aa83a5d5");
        /// <summary>
        ///WS_Перемычка
        ///Двери, окна/Экземпляр/Текст (Параметр проекта)
        /// </summary>
        public static Guid WS_Peremychka = new Guid("e5038948-a77b-4e51-bc55-9a15089ed984");
        /// <summary>
        ///Высота
        /// </summary>
        public static Guid Vysota = new Guid("75d4af48-094c-4799-8e1b-b0cb2d3b1099");
        /// <summary>
        ///Задание ОВ
        /// </summary>
        public static Guid Zadanie_OV = new Guid("c7d2d048-2b76-4a9a-99db-9007ea09a9a4");
        /// <summary>
        ///Суффикс прибора
        /// </summary>
        public static Guid Suffiks_Pribora = new Guid("d0254649-77ce-4d06-a922-1e51028965e0");
        /// <summary>
        ///W_Окна_Толшина стены
        ///<Используется для подсчета площади откосов>
        /// </summary>
        public static Guid W_Okna_Tolshina_Steny = new Guid("e0fa5749-87f3-4ab2-a0de-183560352940");
        /// <summary>
        ///Номер_клавиши_выключателей
        /// </summary>
        public static Guid Nomer_Klavishi_Vyklyuchateley = new Guid("2d676749-1f48-440b-8909-83d3488ae995");
        /// <summary>
        ///W_Шифр проекта (стадия Р)
        ///Экземпляр/Текст (Параметр проекта)
        /// </summary>
        public static Guid W_SHifr_Proekta_Stadiya_R = new Guid("9a85f549-5f60-42c7-8e91-6a716214e5a6");
        /// <summary>
        ///W_Добор_Длина
        /// </summary>
        public static Guid W_Dobor_Dlina = new Guid("7db82f4a-150d-4745-ba0b-8d8219bba187");
        /// <summary>
        ///W_Перемычки_лист_s
        /// </summary>
        public static Guid W_Peremychki_List_s = new Guid("f7f85a4a-d50b-4837-8ab3-79fa86eb0fa8");
        /// <summary>
        ///W_Откосы_Вн_У стены_Высота
        /// </summary>
        public static Guid W_Otkosy_Vn_U_Steny_Vysota = new Guid("2177924a-4389-4f67-bb56-b33dbe04066e");
        /// <summary>
        ///WSH_Площадь с учетом отделки
        ///Помещения/Экземпляр/Размеры (Параметр проекта)
        /// </summary>
        public static Guid WSH_Ploshchad_S_Uchetom_Otdelki = new Guid("56eb084b-600c-4871-9301-88eb59a498f2");
        /// <summary>
        ///Ток 3КЗ, A
        /// </summary>
        public static Guid Tok_3KZ_A = new Guid("2202784b-0ccb-4569-91b7-6f8d98fb7b72");
        /// <summary>
        ///W_Откосы_Вн_У заполнения_Ширина
        /// </summary>
        public static Guid W_Otkosy_Vn_U_Zapolneniya_SHirina = new Guid("0a98ca4b-6597-49f9-8017-fdc3684fa186");
        /// <summary>
        ///Изменения
        /// </summary>
        public static Guid Izmeneniya = new Guid("34ecdb4b-2ab1-4b5e-b1cb-7c592e489082");
        /// <summary>
        ///W_ЭлемФасада_Зона
        /// </summary>
        public static Guid W_ElemFasada_Zona = new Guid("58830e4c-4ec9-4f9e-b0b0-e353f62d2b24");
        /// <summary>
        ///W_Потолок_Тип
        ///Экземпляр/Строительство (Параметр проекта)
        /// </summary>
        public static Guid W_Potolok_Tip = new Guid("86e1694c-05c4-450d-8304-4d198a3dc0b6");
        /// <summary>
        ///Кол-во подключения
        /// </summary>
        public static Guid Kolvo_Podklyucheniya = new Guid("c35e6f4c-3abd-4a40-b241-8586b5d8bfeb");
        /// <summary>
        ///Крепление к стене (вертикальный лоток)
        /// </summary>
        public static Guid Kreplenie_K_Stene_Vertikalnyy_Lotok = new Guid("dc68ff4c-278b-4578-83fb-66405dcced67");
        /// <summary>
        ///Строка 4 должность
        /// </summary>
        public static Guid Stroka_4_Dolzhnost = new Guid("c0f4a14d-0a30-4e08-a233-2a1a23db2403");
        /// <summary>
        ///Принадлежность
        /// </summary>
        public static Guid Prinadlezhnost = new Guid("dc4eb84d-a82b-4bad-90a2-71ec8fbd72c2");
        /// <summary>
        ///Кол-во крепежа
        /// </summary>
        public static Guid Kolvo_Krepezha = new Guid("e992c04d-6820-40fa-9752-b3b06c3b1c28");
        /// <summary>
        ///W_Перемычки_лист_марка_стали
        /// </summary>
        public static Guid W_Peremychki_List_Marka_Stali = new Guid("1e864a4e-f0f3-4282-88ba-ac58b71d3925");
        /// <summary>
        ///W_Тип стены (основа)
        ///Экземпляр/Результаты анализа
        /// </summary>
        public static Guid W_Tip_Steny_Osnova = new Guid("718d954e-fa76-4927-aef7-10ed0ea38b38");
        /// <summary>
        ///Интервал крепления к потолку
        /// </summary>
        public static Guid Interval_Krepleniya_K_Potolku = new Guid("2850064f-3556-4c4d-9a44-9357cac2f39c");
        /// <summary>
        ///тест
        /// </summary>
        public static Guid test = new Guid("3a33ba4f-2ee4-4e60-b898-6a694099901f");
        /// <summary>
        ///Номер_АР
        /// </summary>
        public static Guid Nomer_AR = new Guid("4a61c44f-b09b-438d-8753-845e443f8b89");
        /// <summary>
        ///Выбросы/сбросы
        /// </summary>
        public static Guid Vybrosysbrosy = new Guid("6c96fa4f-4f05-476b-a2b5-a9299014111d");
        /// <summary>
        ///W_Откосы_Нар_У заполнения_Ширина
        /// </summary>
        public static Guid W_Otkosy_Nar_U_Zapolneniya_SHirina = new Guid("aee81850-8422-44ab-84cc-6f8fc2bbf898");
        /// <summary>
        ///WS_Тип стены
        ///Экземпляр/Результаты анализа
        /// </summary>
        public static Guid WS_Tip_Steny = new Guid("ebba1b50-c5c4-4fcd-a059-07f907e493ec");
        /// <summary>
        ///Наполнение листа
        /// </summary>
        public static Guid Napolnenie_Lista = new Guid("9cc46250-fe68-456e-95c2-40f051b65ac3");
        /// <summary>
        ///Водоподготовка
        /// </summary>
        public static Guid Vodopodgotovka = new Guid("5f157250-df3b-43f3-bfd6-b69df3ded8cf");
        /// <summary>
        ///Пневмопочта
        /// </summary>
        public static Guid Pnevmopochta = new Guid("3df1cb50-7635-437d-a6b2-40a54b9a0d05");
        /// <summary>
        ///Перегородка
        /// </summary>
        public static Guid Peregorodka = new Guid("8e5ae950-73d7-4543-9a95-662253b6f9c0");
        /// <summary>
        ///W_Шифр проекта_Стадия П
        ///Листы/Текст (Общий параметр)
        /// </summary>
        public static Guid W_SHifr_Proekta_Stadiya_P = new Guid("621d0651-f10a-442a-94f6-c7e45682c06f");
        /// <summary>
        ///W_Перемычка_Поз_уголок
        /// </summary>
        public static Guid W_Peremychka_Poz_Ugolok = new Guid("3b942051-907a-428b-a399-3582b9f5b096");
        /// <summary>
        ///W_Лестницы_Отделка низа маршей_Площадь
        ///Лестницы/Экземпляр/Материалы и отделка (Параметр проекта)
        /// </summary>
        public static Guid W_Lestnitsy_Otdelka_Niza_Marshey_Ploshchad = new Guid("f24f6951-7e2c-4ed5-82fc-eb2bf0ed4945");
        /// <summary>
        ///W_Крепежный элемент_Кол-во
        /// </summary>
        public static Guid W_Krepezhnyy_Element_Kolvo = new Guid("6c15bc51-efcc-42c1-a008-86fb13f378d5");
        /// <summary>
        ///ID связанного элемента
        /// </summary>
        public static Guid ID_Svyazannogo_Elementa = new Guid("dca1fe51-4090-4178-9f12-a83aa5986266");
        /// <summary>
        ///6_Номер изменения
        /// </summary>
        public static Guid Nomer_Izmeneniya6_ = new Guid("c0d1be52-3350-44c7-9775-8d902ccf7e62");
        /// <summary>
        ///Номер листа вручную (текст)
        /// </summary>
        public static Guid Nomer_Lista_Vruchnuyu_Tekst = new Guid("7a244e53-ebb4-4073-b72e-ecfadb04e3c1");
        /// <summary>
        ///Высота проёма
        /// </summary>
        public static Guid Vysota_Proema = new Guid("95f18b53-d3bd-4b37-a741-d6b7c0c6f7ae");
        /// <summary>
        ///9_Лист
        /// </summary>
        public static Guid List9_ = new Guid("5640d253-9ca6-4a80-945c-65f81a4fd349");
        /// <summary>
        ///Канализация
        /// </summary>
        public static Guid Kanalizatsiya = new Guid("7fdffe53-ec78-4134-9ffd-4be07c4ef2ec");
        /// <summary>
        ///Уровень звукового давления в помещении
        /// </summary>
        public static Guid Uroven_Zvukovogo_Davleniya_V_Pomeshchenii = new Guid("7b16ff53-0d73-4739-8890-01feaa8b3a87");
        /// <summary>
        ///8_Лист
        /// </summary>
        public static Guid List8_ = new Guid("60972754-913d-4db4-87dd-75a89e9ba839");
        /// <summary>
        ///Rд дуги
        /// </summary>
        public static Guid Rd_Dugi = new Guid("d1df6d54-7620-4295-85f1-f8083961da61");
        /// <summary>
        ///Завод изготоитель
        /// </summary>
        public static Guid Zavod_Izgotoitel = new Guid("17f5e454-a243-45b8-807d-69251c9ba104");
        /// <summary>
        ///W_Пожарная секция
        ///Помещения/Экземпляр/Текст (Параметр проекта)
        /// </summary>
        public static Guid W_Pozharnaya_Sektsiya = new Guid("1a779255-610b-4d54-8b80-6e4e4120a140");
        /// <summary>
        ///W_Демонтаж отделки
        /// </summary>
        public static Guid W_Demontazh_Otdelki = new Guid("871da955-3e2f-4b96-bc29-b6cd7d7b6164");
        /// <summary>
        ///Номер, имя
        /// </summary>
        public static Guid Nomer_Imya = new Guid("83e9dc55-72e8-4ff2-9c40-9659829b4c55");
        /// <summary>
        ///Выделение вредностей
        /// </summary>
        public static Guid Vydelenie_Vrednostey = new Guid("f12a2656-6071-4975-8b66-fc744055effc");
        /// <summary>
        ///Тип питания пациентов
        /// </summary>
        public static Guid Tip_Pitaniya_Patsientov = new Guid("5f9f6656-a955-4439-93c3-dcc9be1bb340");
        /// <summary>
        ///W_Двери_Обозначение_Порог
        /// </summary>
        public static Guid W_Dveri_Oboznachenie_Porog = new Guid("63286d56-0f6f-48e1-9b55-2e9fd5c98c0e");
        /// <summary>
        ///Максимальный ток ОУ на группах в щитах
        /// </summary>
        public static Guid Maksimalnyy_Tok_OU_Na_Gruppakh_V_SHCHitakh = new Guid("6add9756-43f3-4d11-8bc5-01e13586cff6");
        /// <summary>
        ///W_Профиль_Эскиз
        /// </summary>
        public static Guid W_Profil_Eskiz = new Guid("d2dfe056-9fa3-47a9-baae-01cd3ead7d8e");
        /// <summary>
        ///Масса изд. №2
        /// </summary>
        public static Guid Massa_Izd_2 = new Guid("b3b87657-8759-4961-b5b5-98af4b78f11c");
        /// <summary>
        ///A
        /// </summary>
        public static Guid A = new Guid("b8509157-6acc-4e29-b704-d66097425c4a");
        /// <summary>
        ///W_Потолок_Высота переменная
        /// </summary>
        public static Guid W_Potolok_Vysota_Peremennaya = new Guid("85519b57-e0b2-488d-9d7d-c5b070d7d11e");
        /// <summary>
        ///W_Чистые боксы
        ///Помещения/Экземпляр/Текст (Параметр проекта)
        /// </summary>
        public static Guid W_CHistye_Boksy = new Guid("e518de57-41ab-4e23-9125-be5640d7d5fa");
        /// <summary>
        ///W_ЭлемФасада_Наименование
        /// </summary>
        public static Guid W_ElemFasada_Naimenovanie = new Guid("6ef2df57-4f46-456e-ae60-f81459bf8009");
        /// <summary>
        ///ID
        /// </summary>
        public static Guid ID = new Guid("3a59fd57-fbb6-4a6d-bccc-09cb2b498cc2");
        /// <summary>
        ///W_Перемычки_уголок_s
        /// </summary>
        public static Guid W_Peremychki_Ugolok_s = new Guid("4f171b58-ed5f-4080-9b56-39ccf546e24c");
        /// <summary>
        ///Смещение УГО Х
        /// </summary>
        public static Guid Smeshchenie_UGO_KH = new Guid("19024358-5fc9-43ca-aded-b202ff23c740");
        /// <summary>
        ///Номер помещения
        /// </summary>
        public static Guid Nomer_Pomeshcheniya = new Guid("13d14d58-6184-449e-aacf-e47a7e32957f");
        /// <summary>
        ///Масса изд. общая
        /// </summary>
        public static Guid Massa_Izd_Obshchaya = new Guid("58427658-413f-4a82-86b0-68a634ca51e2");
        /// <summary>
        ///W_Площадь сечения
        /// </summary>
        public static Guid W_Ploshchad_Secheniya = new Guid("66dba258-1226-4231-b10e-75366c2d84cd");
        /// <summary>
        ///Тип, марка, обозначение документа
        /// </summary>
        public static Guid Tip_Marka_Oboznachenie_Dokumenta = new Guid("f3b4a558-4a36-47cc-8798-b27f13d99fb0");
        /// <summary>
        ///УГО_вертикальное_смещение
        /// </summary>
        public static Guid UGO_Vertikalnoe_Smeshchenie = new Guid("af5ca858-7883-415a-b889-5571c2d8b40f");
        /// <summary>
        ///W_Перемычка_Марка
        /// </summary>
        public static Guid W_Peremychka_Marka = new Guid("19e20559-4cee-4133-b04e-3476654c2233");
        /// <summary>
        ///Полная мощность в щитах
        /// </summary>
        public static Guid Polnaya_Moshchnost_V_SHCHitakh = new Guid("4a103159-3f48-4fd8-a827-492effa28522");
        /// <summary>
        ///Категория надежности электроснабжения
        /// </summary>
        public static Guid Kategoriya_Nadezhnosti_Elektrosnabzheniya = new Guid("0e483159-fcac-4a8b-be1c-1e9b24b6afa8");
        /// <summary>
        ///WS_ВОР_Конструкция пола
        ///Помещения/Экземпляр/Прочее (Параметр проекта)
        /// </summary>
        public static Guid WS_VOR_Konstruktsiya_Pola = new Guid("9a699359-1097-4749-8351-bfa2a9164477");
        /// <summary>
        ///Номер, имя помещения
        /// </summary>
        public static Guid Nomer_Imya_Pomeshcheniya = new Guid("b3b1eb59-d0b5-4acf-9de8-537066125149");
        /// <summary>
        ///3_Лист
        /// </summary>
        public static Guid List3_ = new Guid("f80df859-3be2-4bba-a1be-ad6ab71f1988");
        /// <summary>
        ///W_Окна_Вес (кг)
        ///Окна/Тип/Строительство (Параметр семейства)
        /// </summary>
        public static Guid W_Okna_Ves_Kg = new Guid("6861295a-45fc-4f01-b98c-b2eb0c0221ad");
        /// <summary>
        ///5_Номер документа
        /// </summary>
        public static Guid Nomer_Dokumenta5_ = new Guid("949f2d5a-d480-444a-8656-7d6e23ea6825");
        /// <summary>
        ///WS_Усиление/Перемычка_Длина
        ///Двери, окна/Экземпляр/Текст (Параметр проекта)
        /// </summary>
        public static Guid WS_UsileniePeremychka_Dlina = new Guid("7e092e5a-38ce-44f8-bae5-0d188629aca2");
        /// <summary>
        ///W_Отлив_Площадь
        /// </summary>
        public static Guid W_Otliv_Ploshchad = new Guid("fac54b5a-a341-4905-a19b-f38bbd3c0b44");
        /// <summary>
        ///Требования пожарной безопасности
        /// </summary>
        public static Guid Trebovaniya_Pozharnoy_Bezopasnosti = new Guid("0f248a5a-6caf-4446-8c11-2d15964c2608");
        /// <summary>
        ///Порог
        /// </summary>
        public static Guid Porog = new Guid("9872f65a-e13b-4294-9665-7c6672d71541");
        /// <summary>
        ///W_Тип привода
        ///Двери/Тип/Зависимости (Параметр семейства)
        /// </summary>
        public static Guid W_Tip_Privoda = new Guid("41ac185b-21e3-47d7-b965-3f1e2fa00397");
        /// <summary>
        ///Тип лотка - Ширина
        /// </summary>
        public static Guid Tip_Lotka__SHirina = new Guid("12a3375b-115c-4aac-b2a9-a00af9c37f85");
        /// <summary>
        ///Удельная мощность
        ///Удельная мощность, Вт/м
        /// </summary>
        public static Guid Udelnaya_Moshchnost = new Guid("3960205c-59f1-47ce-b785-19b0875cf128");
        /// <summary>
        ///Установленная мощность
        /// </summary>
        public static Guid Ustanovlennaya_Moshchnost = new Guid("9ebba55d-0d75-4556-8fcf-93b5362c3e27");
        /// <summary>
        ///Медгазоснабжение
        /// </summary>
        public static Guid Medgazosnabzhenie = new Guid("dfa8ea5d-bd23-4bff-bdfc-39d9fcbca906");
        /// <summary>
        ///W_Перемычки_уголок_a
        /// </summary>
        public static Guid W_Peremychki_Ugolok_a = new Guid("349ff55d-e0f7-47b0-bad3-36e234e9fb1c");
        /// <summary>
        ///Отм.
        /// </summary>
        public static Guid Otm = new Guid("cc8ff75d-37c7-44bc-829d-2f7cd5cad898");
        /// <summary>
        ///УГО_горизонтальное_смещение
        /// </summary>
        public static Guid UGO_Gorizontalnoe_Smeshchenie = new Guid("d50a0e5f-3ce5-4fc3-aa71-a0557800e2ad");
        /// <summary>
        ///Скобы
        /// </summary>
        public static Guid Skoby = new Guid("e3ea825f-c2dc-4d15-9015-7f928774479a");
        /// <summary>
        ///W_Двери_Маркировка проёма
        /// </summary>
        public static Guid W_Dveri_Markirovka_Proema = new Guid("1ee1d15f-9709-4068-b3a6-e75747399477");
        /// <summary>
        ///Задание СС
        /// </summary>
        public static Guid Zadanie_SS = new Guid("1fcf0460-6381-4124-867b-5ff5723de151");
        /// <summary>
        ///W_Кабина_Тип (Проходная/Непроходная)
        ///Оборудование/Экземпляр/Механизмы (Параметр семейства)
        /// </summary>
        public static Guid W_Kabina_Tip_ProkhodnayaNeprokhodnaya = new Guid("a22a0660-ec68-4e91-a2d9-568f64f0347e");
        /// <summary>
        ///Изделие №3
        /// </summary>
        public static Guid Izdelie_3 = new Guid("4aeb1060-124c-4a90-b747-20bf5a3c50cf");
        /// <summary>
        ///Блок отделения
        /// </summary>
        public static Guid Blok_Otdeleniya = new Guid("acca2c60-13bb-4db0-bdde-2eb0c221b5a6");
        /// <summary>
        ///Лотки
        /// </summary>
        public static Guid Lotki = new Guid("28dc6c60-32c7-4dfd-a42e-eb77e9939c08");
        /// <summary>
        ///W_Двери_Примечания_Притвор
        /// </summary>
        public static Guid W_Dveri_Primechaniya_Pritvor = new Guid("ff86bd60-a6d5-4646-878e-1ef514e8aad0");
        /// <summary>
        ///W_Примечание
        ///Экземпляр/? (Параметр проекта)
        /// </summary>
        public static Guid W_Primechanie = new Guid("e92d1361-60b0-4df7-947f-5288f862b75f");
        /// <summary>
        ///Контур
        /// </summary>
        public static Guid Kontur = new Guid("e7fa2161-b0da-4e07-bd61-33b3e45a4b13");
        /// <summary>
        ///ADSK_Примечание
        ///Текстовое поле для заполнения ячейки "Примечание" в таблицах спецификаций
        /// </summary>
        public static Guid ADSK_Primechanie = new Guid("a85b7661-26b0-412f-979c-66af80b4b2c3");
        /// <summary>
        ///W_Двери_Обозначение_Наружняя/Внутренняя
        /// </summary>
        public static Guid W_Dveri_Oboznachenie_NaruzhnyayaVnutrennyaya = new Guid("faba8061-26b7-4d9c-9bf7-b834c4a68205");
        /// <summary>
        ///WH_Сортировка вида (Раздел проекта)
        /// </summary>
        public static Guid WH_Sortirovka_Vida_Razdel_Proekta = new Guid("5ff49461-40db-4d79-89c4-ba916ae5cd08");
        /// <summary>
        ///W_Описание_Тип
        /// </summary>
        public static Guid W_Opisanie_Tip = new Guid("b9a9b461-92d9-440d-9a04-48d0b09b4605");
        /// <summary>
        ///WS_Фартук_Площадь
        ///Экземпляр/Строительство (Параметр проекта)
        /// </summary>
        public static Guid WS_Fartuk_Ploshchad = new Guid("bc18ca61-a5bc-4217-a5bb-782072a9b161");
        /// <summary>
        ///AR_Перемычка_Толщина сердцевины
        ///Толщина сердцевины стены
        /// </summary>
        public static Guid AR_Peremychka_Tolshchina_Serdtseviny = new Guid("32c6f461-8fb2-4911-b05f-8d7c238d3595");
        /// <summary>
        ///Ток 1КЗ, А
        /// </summary>
        public static Guid Tok_1KZ_A = new Guid("65444762-6412-41d3-b808-2d3934df9a9b");
        /// <summary>
        ///W_Перемычки_Позиция_1
        /// </summary>
        public static Guid W_Peremychki_Pozitsiya_1 = new Guid("4c395462-c39f-42be-9a54-27b81cbeb631");
        /// <summary>
        ///W_Длина
        /// </summary>
        public static Guid W_Dlina = new Guid("d9515c62-8ea5-460f-910a-64ac55c54a55");
        /// <summary>
        ///Изменение 6
        /// </summary>
        public static Guid Izmenenie_6 = new Guid("e6b4b962-28a8-4880-9606-d515fa19005c");
        /// <summary>
        ///W_Потолок_Тип сущ. отделки
        /// </summary>
        public static Guid W_Potolok_Tip_Sushch_Otdelki = new Guid("a476dc62-8e22-4fcc-bfe9-55d6afafac46");
        /// <summary>
        ///W_Мрк_Наименование
        /// </summary>
        public static Guid W_Mrk_Naimenovanie = new Guid("ed204f63-d86d-410c-aae4-a024b576d83d");
        /// <summary>
        ///WS_Усиление/Перемычка_Вес
        ///Двери, окна/Экземпляр/Текст (Параметр проекта)
        /// </summary>
        public static Guid WS_UsileniePeremychka_Ves = new Guid("cedab463-3090-45bf-b7eb-8af45a623925");
        /// <summary>
        ///W_Сечение_Высота
        /// </summary>
        public static Guid W_Sechenie_Vysota = new Guid("b419f163-85dc-446b-bea8-493ef9693658");
        /// <summary>
        ///Ав освещение
        /// </summary>
        public static Guid Av_Osveshchenie = new Guid("70702364-1bca-4f7f-b747-a2c9b6b55a89");
        /// <summary>
        ///Ширина внутренней грани ниши
        /// </summary>
        public static Guid SHirina_Vnutrenney_Grani_Nishi = new Guid("7f902864-9b25-4098-ac11-e14b375fc8d6");
        /// <summary>
        ///Тип лотка - Высота
        /// </summary>
        public static Guid Tip_Lotka__Vysota = new Guid("5bea5e64-876e-45fa-a6c1-e40a53f9c98a");
        /// <summary>
        ///W_Подоконник_Длина
        ///Окна/Тип/Размеры (Параметр семейства)
        /// </summary>
        public static Guid W_Podokonnik_Dlina = new Guid("28766364-e7ea-4dae-8577-187fb1d0da48");
        /// <summary>
        ///W_Двери_Примечание_вручную
        ///Поле для вставки информации непридусмотренной стандартными примечаниями
        /// </summary>
        public static Guid W_Dveri_Primechanie_Vruchnuyu = new Guid("0091dd64-8286-409a-b04f-13314e0ec542");
        /// <summary>
        ///W_Внешняя ширина откоса на плоскости рамы_(основание-кирпич)
        /// </summary>
        public static Guid W_Vneshnyaya_SHirina_Otkosa_Na_Ploskosti_Ramy_Osnovaniekirpich = new Guid("c48bf664-3030-48c1-b0ca-2063ab27995b");
        /// <summary>
        ///W_Пол_Дефекты
        /// </summary>
        public static Guid W_Pol_Defekty = new Guid("88931c65-766d-4d82-aa94-1bb43e98ab6c");
        /// <summary>
        ///Количество модулей в щитах
        /// </summary>
        public static Guid Kolichestvo_Moduley_V_SHCHitakh = new Guid("452e5665-0b1c-4513-a7b4-6b91a2bef7bd");
        /// <summary>
        ///_Кратность притока
        /// </summary>
        public static Guid Kratnost_Pritoka_ = new Guid("dc797c65-cbba-4056-9316-2669d8425d95");
        /// <summary>
        ///Температура
        /// </summary>
        public static Guid Temperatura = new Guid("66b97d65-4dc5-44c3-a1e9-314a2f9ca67a");
        /// <summary>
        ///WS_Стены_Площадь ГБ
        ///Экземпляр/Аналитическая модель (Параметр проекта)
        /// </summary>
        public static Guid WS_Steny_Ploshchad_GB = new Guid("68b4bc65-ca02-4f8e-a80f-6968af3f0fa7");
        /// <summary>
        ///Eng.Name
        /// </summary>
        public static Guid EngName = new Guid("2141d165-0525-4035-a95b-c8f911665271");
        /// <summary>
        ///WS_Марка комплектации
        ///Двери/Экземпляр/Текст (Параметр семейства)
        /// </summary>
        public static Guid WS_Marka_Komplektatsii = new Guid("bc8f5e66-b061-4fbc-b5c7-fd4116e52582");
        /// <summary>
        ///Кол-во изделий №4
        /// </summary>
        public static Guid Kolvo_Izdeliy_4 = new Guid("602f5f66-643a-44bb-bd4d-f5edb6fbca3d");
        /// <summary>
        ///W_Перемычки_уголок_марка_стали
        /// </summary>
        public static Guid W_Peremychki_Ugolok_Marka_Stali = new Guid("2c0c8567-138e-4573-b4a0-20b32e705085");
        /// <summary>
        ///Хомуты для крышки
        /// </summary>
        public static Guid KHomuty_Dlya_Kryshki = new Guid("cdad8f67-2182-4733-a634-244537691e5f");
        /// <summary>
        ///W_Фурнитура
        /// </summary>
        public static Guid W_Furnitura = new Guid("809c9568-bdf9-4b54-8524-19e220ff36e6");
        /// <summary>
        ///W_Наименование профиля
        /// </summary>
        public static Guid W_Naimenovanie_Profilya = new Guid("6d41df68-7d18-4193-a97c-ebea383321c0");
        /// <summary>
        ///Масса изд. №4
        /// </summary>
        public static Guid Massa_Izd_4 = new Guid("45cdf168-0c82-421a-993f-5241cb344cc0");
        /// <summary>
        ///Сопротивление контактов источника (Zпки)
        /// </summary>
        public static Guid Soprotivlenie_Kontaktov_Istochnika_Zpki = new Guid("d02ef868-2749-419f-971c-e41b43f04aa9");
        /// <summary>
        ///Г.в.
        /// </summary>
        public static Guid Gv = new Guid("ec0b0169-5fd1-498d-bf1f-55d6b0d23b3d");
        /// <summary>
        ///Мед.сестер
        /// </summary>
        public static Guid Medsester = new Guid("af266669-287c-4513-876a-4206fbf5f4fe");
        /// <summary>
        ///WH_Левая
        ///Двери/Тип/Зависимости (Параметр семейства)
        /// </summary>
        public static Guid WH_Levaya = new Guid("05828a69-870c-42f6-9d21-94743dda8232");
        /// <summary>
        ///Объем электрического щита
        /// </summary>
        public static Guid Obem_Elektricheskogo_SHCHita = new Guid("e3abbc69-829a-4919-afc2-855c01ddaad8");
        /// <summary>
        ///Резервная группа
        /// </summary>
        public static Guid Rezervnaya_Gruppa = new Guid("cd2dc469-276a-40f4-bd34-c6ab2ae05348");
        /// <summary>
        ///W_КМ_УсилиеN
        /// </summary>
        public static Guid W_KM_UsilieN = new Guid("2efdd469-64f9-4749-94d3-dcbd371dcfce");
        /// <summary>
        ///СКС(кол-во портов)
        /// </summary>
        public static Guid SKSkolvo_Portov = new Guid("dbdaef69-7895-4cc5-8340-60bb1309101b");
        /// <summary>
        ///9_Номер документа
        /// </summary>
        public static Guid Nomer_Dokumenta9_ = new Guid("2daff869-45d7-47a7-8d94-0a74acac0c19");
        /// <summary>
        ///W_Кол-во элементов
        /// </summary>
        public static Guid W_Kolvo_Elementov = new Guid("61ad316a-01a5-4da0-927c-49e404a04c3d");
        /// <summary>
        ///Отметка уровня земли
        /// </summary>
        public static Guid Otmetka_Urovnya_Zemli = new Guid("2fde546a-eac7-4408-85de-a5022a90c298");
        /// <summary>
        ///W_СКУД
        ///Двери/Экземпляр/Текст (Параметр семейства)
        /// </summary>
        public static Guid W_SKUD = new Guid("9f19716a-90bb-46bf-be16-4dbd7cd86914");
        /// <summary>
        ///Интервал крепления к стене
        /// </summary>
        public static Guid Interval_Krepleniya_K_Stene = new Guid("14f68b6a-97d6-4f26-b392-ec4bf668785b");
        /// <summary>
        ///Дата 4
        /// </summary>
        public static Guid Data_4 = new Guid("90a9a06a-87ef-4fdc-b540-712f2e2db1ff");
        /// <summary>
        ///W_Ширина проёма под короб_(с учётом зазоров)
        /// </summary>
        public static Guid W_SHirina_Proema_Pod_Korob_S_Uchetom_Zazorov = new Guid("5394bc6a-8412-422b-8d94-3dfc7f24ec68");
        /// <summary>
        ///W_Проем в свету_Ширина
        /// </summary>
        public static Guid W_Proem_V_Svetu_SHirina = new Guid("28d8e76a-6dff-435b-9474-3d98e9535f3c");
        /// <summary>
        ///Толщина простенка
        /// </summary>
        public static Guid Tolshchina_Prostenka = new Guid("677f6f6b-1f16-46c4-9a95-9d13eb472418");
        /// <summary>
        ///Высота установки
        /// </summary>
        public static Guid Vysota_Ustanovki = new Guid("b657986b-97b5-42d2-84d4-d31e6f2daa4e");
        /// <summary>
        ///Активная мощность в щитах
        /// </summary>
        public static Guid Aktivnaya_Moshchnost_V_SHCHitakh = new Guid("1a63996b-777a-471f-aa56-b91d1c1f7232");
        /// <summary>
        ///ID Помещения
        ///Помещения,Перекрытия/Экземпляр/Идентификация (Параметр проекта)
        /// </summary>
        public static Guid ID_Pomeshcheniya = new Guid("72bece6b-c91b-4284-be73-2ae45a020429");
        /// <summary>
        ///Ток
        /// </summary>
        public static Guid Tok = new Guid("e5502a6c-722c-4494-9233-0c9b9ebb8bc4");
        /// <summary>
        ///Мощность
        /// </summary>
        public static Guid Moshchnost = new Guid("048f606c-a56f-410b-b8ab-370c28abd1fe");
        /// <summary>
        ///W_Перемычки_масса_уголок
        /// </summary>
        public static Guid W_Peremychki_Massa_Ugolok = new Guid("a83e716c-45e8-4f0c-bee4-2b19f0d7cb67");
        /// <summary>
        ///W_Заполнение_Площадь
        /// </summary>
        public static Guid W_Zapolnenie_Ploshchad = new Guid("9f47806c-ddbd-4e9a-96e5-216626a63083");
        /// <summary>
        ///W_Отлив_Описание
        /// </summary>
        public static Guid W_Otliv_Opisanie = new Guid("a4fc9b6c-b9f1-4589-b4e0-795b5f8d9712");
        /// <summary>
        ///W_Двери_Обозначение_Класс прочности
        /// </summary>
        public static Guid W_Dveri_Oboznachenie_Klass_Prochnosti = new Guid("97f53a6d-0c6d-46a5-83b7-aa0e5dadaaf0");
        /// <summary>
        ///Потеря напряжения для ОС
        /// </summary>
        public static Guid Poterya_Napryazheniya_Dlya_OS = new Guid("b4954a6d-3d42-44ff-b700-e308cf0fcc46");
        /// <summary>
        ///№ на листе
        /// </summary>
        public static Guid _Na_Liste = new Guid("5991736d-26a7-4316-bc45-668afe3354ca");
        /// <summary>
        ///2_Количество участков
        /// </summary>
        public static Guid Kolichestvo_Uchastkov2_ = new Guid("1c4f006e-cc4f-4356-9b0e-5b32f8ad576f");
        /// <summary>
        ///Строка 2 должность
        /// </summary>
        public static Guid Stroka_2_Dolzhnost = new Guid("053e616e-cb3c-4dc5-8348-1c407e8e13a6");
        /// <summary>
        ///Шахта_Высота
        /// </summary>
        public static Guid SHakhta_Vysota = new Guid("2a89f56e-07b4-4271-b4b3-8da8da3f6681");
        /// <summary>
        ///W_Проем_Площадь
        /// </summary>
        public static Guid W_Proem_Ploshchad = new Guid("b0f2446f-4642-427c-8ed5-be7a8d799b64");
        /// <summary>
        ///W_Высота подъема
        ///Оборудование/Тип/Размеры (Параметр семейства) &#xD&#xA- Полная высота подъема
        /// </summary>
        public static Guid W_Vysota_Podema = new Guid("c8cc886f-e351-4a0d-af07-d91aade97447");
        /// <summary>
        ///WSH_Площадь нормативная
        ///Помещения/Экземпляр/Размеры (Параметр проекта)
        /// </summary>
        public static Guid WSH_Ploshchad_Normativnaya = new Guid("3fe0ba6f-9218-41b7-b821-9b6ce5407fa2");
        /// <summary>
        ///W_Перемычки_уголок_b
        /// </summary>
        public static Guid W_Peremychki_Ugolok_b = new Guid("cecacb6f-099e-4ff9-be10-b64fad84a486");
        /// <summary>
        ///Объект
        /// </summary>
        public static Guid Obekt = new Guid("e54b0070-0d90-4ddb-8894-37064aa058ee");
        /// <summary>
        ///Розетка телевизионная
        /// </summary>
        public static Guid Rozetka_Televizionnaya = new Guid("53684e70-d8a2-4376-87d9-24bc1f410c76");
        /// <summary>
        ///Наличие естественного освещения
        /// </summary>
        public static Guid Nalichie_Estestvennogo_Osveshcheniya = new Guid("fe087b70-600b-486f-8dc5-061cb1ebd59e");
        /// <summary>
        ///Высота щита
        /// </summary>
        public static Guid Vysota_SHCHita = new Guid("06d88b70-f669-4fc6-822f-6e4579de4016");
        /// <summary>
        ///Стадия
        /// </summary>
        public static Guid Stadiya = new Guid("041fc070-7b59-4603-ac0f-1c74c2cb67d0");
        /// <summary>
        ///7_Лист
        /// </summary>
        public static Guid List7_ = new Guid("6ce8f070-eff3-4b2c-96f4-6dc575e4e1fe");
        /// <summary>
        ///W_Видимость заливки
        /// </summary>
        public static Guid W_Vidimost_Zalivki = new Guid("a4824871-ccc8-4b58-a543-f4744325f518");
        /// <summary>
        ///W_Двери_Полотно_Отметка остекления
        /// </summary>
        public static Guid W_Dveri_Polotno_Otmetka_Ostekleniya = new Guid("c6e44871-1795-41b0-bfa2-cbfed6dfffd2");
        /// <summary>
        ///4_Номер изменения
        /// </summary>
        public static Guid Nomer_Izmeneniya4_ = new Guid("d64b5571-6640-4a21-97e8-ac93fc664802");
        /// <summary>
        ///Ток в щитах
        /// </summary>
        public static Guid Tok_V_SHCHitakh = new Guid("86d2b171-cfb3-4fcf-811f-38d9a253a297");
        /// <summary>
        ///Подписи_Видимость
        /// </summary>
        public static Guid Podpisi_Vidimost = new Guid("adf0c771-1824-4722-afc3-186095238216");
        /// <summary>
        ///_Система Притока
        /// </summary>
        public static Guid Sistema_Pritoka_ = new Guid("b2399b72-5bd3-44d7-97fa-e440eb7f2146");
        /// <summary>
        ///Тепловыделение от электрооборудования
        /// </summary>
        public static Guid Teplovydelenie_Ot_Elektrooborudovaniya = new Guid("6372d372-2db1-4040-97e6-7951399aa608");
        /// <summary>
        ///Ширина щита
        /// </summary>
        public static Guid SHirina_SHCHita = new Guid("89f05073-5d72-46fb-99ab-2967c9057249");
        /// <summary>
        ///Дата 2
        /// </summary>
        public static Guid Data_2 = new Guid("3499ab73-d8b2-4b2e-8f77-621372b58d32");
        /// <summary>
        ///К1 площадь з.п.
        /// </summary>
        public static Guid K1_Ploshchad_Zp = new Guid("25393e74-0913-4dce-884b-6423d6309ec1");
        /// <summary>
        ///b - Длина
        /// </summary>
        public static Guid b__Dlina = new Guid("5f695b74-4a5c-4e93-ae9a-bc368f94c7cd");
        /// <summary>
        ///Задание
        /// </summary>
        public static Guid Zadanie = new Guid("a75fba74-158f-4fd3-a85a-cf15eef845b4");
        /// <summary>
        ///Запретить изменение
        /// </summary>
        public static Guid Zapretit_Izmenenie = new Guid("be64f474-c030-40cf-9975-6eaebe087a84");
        /// <summary>
        ///Внешний диаметр трубы
        /// </summary>
        public static Guid Vneshniy_Diametr_Truby = new Guid("e0183875-a341-40f2-932f-08258d67138d");
        /// <summary>
        ///ID Temp
        /// </summary>
        public static Guid ID_Temp = new Guid("62ef3a75-5fca-4f9b-9935-4f2c30922756");
        /// <summary>
        ///КЛ - Масса, кг
        /// </summary>
        public static Guid KL__Massa_Kg = new Guid("6d043b75-0132-4541-ae1f-42b8494491ca");
        /// <summary>
        ///WS_Класс чистоты
        ///Помещения/Экземпляр/Текст (Параметр проекта)
        /// </summary>
        public static Guid WS_Klass_CHistoty = new Guid("c17b8f75-ee3f-4afe-a26e-7e73ed00357d");
        /// <summary>
        ///Обработка инструментов_АР
        /// </summary>
        public static Guid Obrabotka_Instrumentov_AR = new Guid("fe92cd75-d674-43b9-8928-28c111033bfe");
        /// <summary>
        ///Строка 5 фамилия
        /// </summary>
        public static Guid Stroka_5_Familiya = new Guid("6153ed75-b3d0-4e9a-ade5-f851a2a4cab6");
        /// <summary>
        ///Потеря напряжения в щитах
        /// </summary>
        public static Guid Poterya_Napryazheniya_V_SHCHitakh = new Guid("e333fe75-fb6c-412d-8506-c9bed9ed860f");
        /// <summary>
        ///Габаритная высота ниши
        /// </summary>
        public static Guid Gabaritnaya_Vysota_Nishi = new Guid("8d867776-f409-4976-8450-66b5d6377f42");
        /// <summary>
        ///Изделие №1
        /// </summary>
        public static Guid Izdelie_1 = new Guid("949e8676-2c6c-4c7d-85fb-56ba3ade0a12");
        /// <summary>
        ///ОВ_Система
        /// </summary>
        public static Guid OV_Sistema = new Guid("ffe2a376-96ae-4afb-bde0-e66b585d3f31");
        /// <summary>
        ///1_Номер изменения
        /// </summary>
        public static Guid Nomer_Izmeneniya1_ = new Guid("5d55ad76-2551-435b-a17d-5aa031336103");
        /// <summary>
        ///4_Дата
        /// </summary>
        public static Guid Data4_ = new Guid("f5a3c076-09ff-4858-b8de-8cd25a05c5b8");
        /// <summary>
        ///Фрагмент
        /// </summary>
        public static Guid Fragment = new Guid("5800dc76-79e0-48a3-ab65-b88eebc59af8");
        /// <summary>
        ///WH_Место установки
        ///Двери/Тип/Зависимости (Параметр семейства)
        /// </summary>
        public static Guid WH_Mesto_Ustanovki = new Guid("d6585977-19e1-413e-a4e6-e6d5cc9eae29");
        /// <summary>
        ///Тип пола 2
        /// </summary>
        public static Guid Tip_Pola_2 = new Guid("caa47c77-bdb4-4f07-978a-5fa4c4f741a9");
        /// <summary>
        ///9_Количество участков
        /// </summary>
        public static Guid Kolichestvo_Uchastkov9_ = new Guid("b80e9577-4fcb-42a1-811f-9b98257c93c0");
        /// <summary>
        ///Максимальная мощность на вывод
        /// </summary>
        public static Guid Maksimalnaya_Moshchnost_Na_Vyvod = new Guid("87821f78-9b90-46fa-b09f-e7d9919ada3c");
        /// <summary>
        ///4_Номер документа
        /// </summary>
        public static Guid Nomer_Dokumenta4_ = new Guid("93e1b378-85a4-48ba-a8dc-d81d65e6fc03");
        /// <summary>
        ///КЛ - Тип, марка
        /// </summary>
        public static Guid KL__Tip_Marka = new Guid("ec772779-df29-49ed-92b6-2781ae489e2f");
        /// <summary>
        ///Ширина внешнего проёма (отчёт)
        /// </summary>
        public static Guid SHirina_Vneshnego_Proema_Otchet = new Guid("2b744f79-1d03-4194-8499-4a5aab7935f1");
        /// <summary>
        ///Количество воздуха
        /// </summary>
        public static Guid Kolichestvo_Vozdukha = new Guid("89a46679-4b32-4ac7-85f6-6dd6b60e8f52");
        /// <summary>
        ///Номер группы по ГОСТ
        /// </summary>
        public static Guid Nomer_Gruppy_Po_GOST = new Guid("8d1b8079-3007-4140-835c-73f0de4e81bd");
        /// <summary>
        ///W_Откос_Вн_У стены_Доп вынос
        /// </summary>
        public static Guid W_Otkos_Vn_U_Steny_Dop_Vynos = new Guid("d6dcee79-1595-45ac-9a5e-f7290f9df0fb");
        /// <summary>
        ///WS_ВОР_Единица измерения_Тип_Масса
        ///Все категории/Тип/Прочее (Параметр проекта)
        /// </summary>
        public static Guid WS_VOR_Edinitsa_Izmereniya_Tip_Massa = new Guid("74dd477a-33e7-4771-b980-c0ea2adcfa56");
        /// <summary>
        ///WH_Площадь пола вручную
        ///<Если площадь пола не равна площади помещения>
        /// </summary>
        public static Guid WH_Ploshchad_Pola_Vruchnuyu = new Guid("fa4a737a-f81b-46b4-aa71-e7165ca6832a");
        /// <summary>
        ///W_Шахта_Ширина
        ///Оборудование/Тип/Размеры (Параметр семейства)
        /// </summary>
        public static Guid W_SHakhta_SHirina = new Guid("964cdb7a-c48e-4477-8127-6fdd322cafa4");
        /// <summary>
        ///Количество полюсов
        /// </summary>
        public static Guid Kolichestvo_Polyusov = new Guid("81b3247b-1892-445f-b738-74b0b0e2b236");
        /// <summary>
        ///Разное
        /// </summary>
        public static Guid Raznoe = new Guid("9e64427b-81e6-4580-a0c9-f30e765f9e6a");
        /// <summary>
        ///W_КМ_УсилиеA
        /// </summary>
        public static Guid W_KM_UsilieA = new Guid("1c684d7b-779a-43de-9be0-6eb37308c1d5");
        /// <summary>
        ///Xт трансформатора
        /// </summary>
        public static Guid Xt_Transformatora = new Guid("3426767b-7ceb-450b-92e5-49d223be4a34");
        /// <summary>
        ///Чистые боксы
        /// </summary>
        public static Guid CHistye_Boksy = new Guid("91dcbd7b-7186-4927-b4c7-c02e92cdf3e2");
        /// <summary>
        ///_Тепл. от оборудования
        /// </summary>
        public static Guid Tepl_Ot_Oborudovaniya_ = new Guid("d2adf27b-174e-47ff-9a3a-a23cd50fde35");
        /// <summary>
        ///Мощность_
        /// </summary>
        public static Guid Moshchnost_ = new Guid("17db257c-eec7-4e8c-b72a-8c133f5fc886");
        /// <summary>
        ///CO2
        /// </summary>
        public static Guid CO2 = new Guid("83807c7c-b664-4249-9d21-75f9cef3c493");
        /// <summary>
        ///Категория ПУЭ
        /// </summary>
        public static Guid Kategoriya_PUE = new Guid("424b157d-a2b1-4bbf-acae-9da0aa94aad1");
        /// <summary>
        ///Пользователи
        /// </summary>
        public static Guid Polzovateli = new Guid("4dc8597d-3a8e-49ee-b722-09757811b411");
        /// <summary>
        ///Курс EUR
        /// </summary>
        public static Guid Kurs_EUR = new Guid("ad4e6a7d-3af4-4d12-afff-269d7d593051");
        /// <summary>
        ///W_Описание конструкции
        ///Тип/Текст (Параметр проекта)
        /// </summary>
        public static Guid W_Opisanie_Konstruktsii = new Guid("8b704a7e-c727-4134-959e-170c16c67ab1");
        /// <summary>
        ///W_Двери_Примечание_Антипаника
        /// </summary>
        public static Guid W_Dveri_Primechanie_Antipanika = new Guid("3ea9697e-a089-4d92-ba7f-c3716fe7cf7b");
        /// <summary>
        ///_Расход воздуха
        /// </summary>
        public static Guid Raskhod_Vozdukha_ = new Guid("29a27a7e-c054-46f7-aea8-838969101436");
        /// <summary>
        ///W_Откосы_Нар_У заполнения_Высота до дуги
        /// </summary>
        public static Guid W_Otkosy_Nar_U_Zapolneniya_Vysota_Do_Dugi = new Guid("c4719d7e-861d-4f3b-8651-4d4012ed8f0c");
        /// <summary>
        ///WH_Площадь потолка вручную
        ///<Если площадь потолка не равна площади помещения>
        /// </summary>
        public static Guid WH_Ploshchad_Potolka_Vruchnuyu = new Guid("942acb7e-a8b6-4277-bfeb-13f7450cd324");
        /// <summary>
        ///Вес
        /// </summary>
        public static Guid Ves = new Guid("58c95e7f-7a45-4d5e-9004-91d1a5a8d5f8");
        /// <summary>
        ///Изделие №5
        /// </summary>
        public static Guid Izdelie_5 = new Guid("dc96b87f-2af0-4492-8f39-98b9172d31a6");
        /// <summary>
        ///5_Номер изменения
        /// </summary>
        public static Guid Nomer_Izmeneniya5_ = new Guid("f501f77f-8a05-4996-aa5c-c93aa515dfb3");
        /// <summary>
        ///Скобы - Интервал установки
        /// </summary>
        public static Guid Skoby__Interval_Ustanovki = new Guid("f18a5b80-04bf-47a0-b0ce-072386d1e47e");
        /// <summary>
        ///W_Высота установки от оси до уровня пола
        /// </summary>
        public static Guid W_Vysota_Ustanovki_Ot_Osi_Do_Urovnya_Pola = new Guid("94788680-171d-4278-81ac-c236c7786262");
        /// <summary>
        ///ЖБ площадь з.п.
        /// </summary>
        public static Guid ZHB_Ploshchad_Zp = new Guid("d857cf80-8e9c-4c10-9acd-86e63aa2adf5");
        /// <summary>
        ///Тепловыделение, Вт
        /// </summary>
        public static Guid Teplovydelenie_Vt = new Guid("a3280881-14d5-4946-ba26-f2b84c4e9401");
        /// <summary>
        ///Наличие источника ионизирующего излучения
        /// </summary>
        public static Guid Nalichie_Istochnika_Ioniziruyushchego_Izlucheniya = new Guid("9c874081-8979-4b9e-ab51-92f1e9184b7e");
        /// <summary>
        ///WS_Минимальная площадь
        ///Экземпляр/Размеры
        /// </summary>
        public static Guid WS_Minimalnaya_Ploshchad = new Guid("b7166f81-0dcf-4ec7-9d56-e24e527387b4");
        /// <summary>
        ///Изменение 9
        /// </summary>
        public static Guid Izmenenie_9 = new Guid("67cc9381-0af8-4ffb-ba5d-89f68291ca41");
        /// <summary>
        ///W_Ширина сечения
        /// </summary>
        public static Guid W_SHirina_Secheniya = new Guid("21cadb81-b935-4460-901c-1ef94a7dd019");
        /// <summary>
        ///Шахта_Ширина
        /// </summary>
        public static Guid SHakhta_SHirina = new Guid("2171df81-6958-4f83-be69-dcd481bbfaa4");
        /// <summary>
        ///Изменения типа
        /// </summary>
        public static Guid Izmeneniya_Tipa = new Guid("e13a6182-f9fc-4910-8d07-65f4e82fcfcf");
        /// <summary>
        ///W_Раковина/Трап_ТХ
        ///Помещения/Экземпляр/Текст (Параметр проекта)
        /// </summary>
        public static Guid W_RakovinaTrap_TKH = new Guid("499fee82-1e3b-4a3f-a37a-351c0f8cabdc");
        /// <summary>
        ///Комментарии изменений типа
        /// </summary>
        public static Guid Kommentarii_Izmeneniy_Tipa = new Guid("550b6383-47c3-448e-9951-63dae8c49ef3");
        /// <summary>
        ///WS_Количество МФУ
        ///Помещения/Экземпляр/Прочее (Параметр проекта)
        /// </summary>
        public static Guid WS_Kolichestvo_MFU = new Guid("ec0df683-440c-45b9-b37f-2a54b86218d2");
        /// <summary>
        ///W_Двери_Обозначение_Заполнение
        ///Глухое/Остекленное/Комбинированное&#xD&#xA
        /// </summary>
        public static Guid W_Dveri_Oboznachenie_Zapolnenie = new Guid("f0271084-d759-43b9-8005-d2e17f00570e");
        /// <summary>
        ///Статус изм.
        /// </summary>
        public static Guid Status_Izm = new Guid("5df64b84-6f28-4f59-a3ee-1de71819d7d0");
        /// <summary>
        ///Строка 1 фамилия
        /// </summary>
        public static Guid Stroka_1_Familiya = new Guid("a856a784-03da-4238-9bd9-9881b9938b8a");
        /// <summary>
        ///Длина конструкции
        /// </summary>
        public static Guid Dlina_Konstruktsii = new Guid("334df084-91f8-475a-b2cf-f5ad92f7d9ca");
        /// <summary>
        ///Сжатый воздух   4 бар
        /// </summary>
        public static Guid Szhatyy_Vozdukh___4_Bar = new Guid("ccb73686-b22f-46a3-9bc7-f6ffbc9b69fd");
        /// <summary>
        ///Наименование нагрузки
        /// </summary>
        public static Guid Naimenovanie_Nagruzki = new Guid("2e466686-a5bd-4329-9427-d0fa03e8742d");
        /// <summary>
        ///WH_Этаж
        ///Двери, окна/Экземпляр/Зависимости (Параметр проекта)
        /// </summary>
        public static Guid WH_Etazh = new Guid("ec72dc86-96ca-4b29-b355-5e572f13377b");
        /// <summary>
        ///Ключевая пометка
        /// </summary>
        public static Guid Klyuchevaya_Pometka = new Guid("4c125487-3dd1-471a-b818-d6d999ccc30a");
        /// <summary>
        ///Дата 3
        /// </summary>
        public static Guid Data_3 = new Guid("ca9c8f87-0b12-4e05-a598-fffb6e35a96e");
        /// <summary>
        ///WS_ВОР_Единица измерения_Доп
        ///Помещения/Экземпляр/Прочее (Параметр проекта)
        /// </summary>
        public static Guid WS_VOR_Edinitsa_Izmereniya_Dop = new Guid("c021d787-9d0f-4e16-bed1-f176ad3c591a");
        /// <summary>
        ///WS_Стены_Площадь ЖБ
        ///Экземпляр/Аналитическая модель (Параметр проекта)
        /// </summary>
        public static Guid WS_Steny_Ploshchad_ZHB = new Guid("0964ef87-f2fa-48a5-9f38-c26a5368d3d8");
        /// <summary>
        ///Ширина канала (отчёт)
        /// </summary>
        public static Guid SHirina_Kanala_Otchet = new Guid("a99a5e88-0774-413b-a0a0-12628eb54e49");
        /// <summary>
        ///Количество кабелей в щитах
        /// </summary>
        public static Guid Kolichestvo_Kabeley_V_SHCHitakh = new Guid("8b477288-32ec-488f-83f5-075733228ae1");
        /// <summary>
        ///Пол. Номер помещения
        /// </summary>
        public static Guid Pol_Nomer_Pomeshcheniya = new Guid("dcadc788-49aa-4a0a-897f-e425381bffde");
        /// <summary>
        ///WH_Жалюзи_Описание
        ///Окна/Экземпляр/Материалы и отделка (Параметр проекта)
        /// </summary>
        public static Guid WH_ZHalyuzi_Opisanie = new Guid("945ccc89-4615-4f67-a70e-610c3e8ec5b7");
        /// <summary>
        ///Потолок
        /// </summary>
        public static Guid Potolok = new Guid("d568f389-9d13-4a56-9588-1a1f754edca8");
        /// <summary>
        ///Вес
        /// </summary>
        public static Guid Ves_1 = new Guid("f406008a-016d-4aa5-bccd-306918020281");
        /// <summary>
        ///Максимальное количество подключаемых устройств
        /// </summary>
        public static Guid Maksimalnoe_Kolichestvo_Podklyuchaemykh_Ustroystv = new Guid("5808358a-d302-40f6-bc62-04f7af0edb34");
        /// <summary>
        ///Категория пространства
        /// </summary>
        public static Guid Kategoriya_Prostranstva = new Guid("3b14818a-d741-43e6-862c-c7654a59a7a0");
        /// <summary>
        ///W_Откосы_Нар_У заполнения_Высота дуги
        /// </summary>
        public static Guid W_Otkosy_Nar_U_Zapolneniya_Vysota_Dugi = new Guid("512e168b-df7b-43d1-a621-41c8a8010839");
        /// <summary>
        ///WS_Наименование
        ///Тип/Группа
        /// </summary>
        public static Guid WS_Naimenovanie = new Guid("75ad308b-3f36-47dd-a0e3-61ac5977a37c");
        /// <summary>
        ///Пол
        /// </summary>
        public static Guid Pol = new Guid("04e0418b-d89a-4f7c-a023-0cb9f7247101");
        /// <summary>
        ///Единица измерения
        /// </summary>
        public static Guid Edinitsa_Izmereniya_1 = new Guid("4a914c8b-2d3b-49eb-bd5c-146b0d37e638");
        /// <summary>
        ///_Врем рабочие места
        /// </summary>
        public static Guid Vrem_Rabochie_Mesta_ = new Guid("a2c6548b-f285-49df-a8f4-be6bef490b94");
        /// <summary>
        ///Обозначение для труб в КЖ
        /// </summary>
        public static Guid Oboznachenie_Dlya_Trub_V_KZH = new Guid("fa69808b-84a5-42d0-996d-72096a42ea2d");
        /// <summary>
        ///Альбом
        /// </summary>
        public static Guid Albom = new Guid("2db4d38b-e5d5-4519-a115-77afe200dd29");
        /// <summary>
        ///W_Диаметр
        /// </summary>
        public static Guid W_Diametr = new Guid("b9abf78b-9ae3-45e5-a6a1-f7d78af4e4d0");
        /// <summary>
        ///10_Лист
        /// </summary>
        public static Guid List10_ = new Guid("c148248c-4d81-410f-86c2-6793206876d4");
        /// <summary>
        ///Марка кабелей для выносок
        /// </summary>
        public static Guid Marka_Kabeley_Dlya_Vynosok = new Guid("78e8268c-f1d6-46c2-8bd9-8c0c0590868a");
        /// <summary>
        ///W_Марка изделия
        /// </summary>
        public static Guid W_Marka_Izdeliya = new Guid("60283f8c-7c8b-44c0-bd87-f9a0d0752f54");
        /// <summary>
        ///FR.type.constraction
        /// </summary>
        public static Guid FRtypeconstraction = new Guid("b8b8448c-6a3d-4f1a-b866-770c9ffeebc6");
        /// <summary>
        ///Rт трансформатора
        /// </summary>
        public static Guid Rt_Transformatora = new Guid("0a324e8c-a6c6-4e07-b439-340ddc0d1f35");
        /// <summary>
        ///1_Лист
        /// </summary>
        public static Guid List1_ = new Guid("8b2e978c-5bd2-4290-83f8-7df2b4164bd7");
        /// <summary>
        ///Строка 4 фамилия
        /// </summary>
        public static Guid Stroka_4_Familiya = new Guid("7f45f58c-1a5e-48f5-b292-bbf312c0bf2e");
        /// <summary>
        ///Установленная мощность в щитах
        /// </summary>
        public static Guid Ustanovlennaya_Moshchnost_V_SHCHitakh = new Guid("9f86038d-9d63-486a-a0c2-9179b24900c8");
        /// <summary>
        ///W_Шифр проекта (стадия П)
        ///Экземпляр/Текст (Параметр проекта)
        /// </summary>
        public static Guid W_SHifr_Proekta_Stadiya_P_1 = new Guid("7ed42b8d-dd3c-4f35-839a-b5bb731a1f9c");
        /// <summary>
        ///Наименование
        /// </summary>
        public static Guid Naimenovanie = new Guid("9870a48d-f575-40f7-b0bd-61372e1a164a");
        /// <summary>
        ///Классификация нагрузки
        /// </summary>
        public static Guid Klassifikatsiya_Nagruzki = new Guid("fa6ed58d-6a37-4060-9cd2-a16c5d110afe");
        /// <summary>
        ///Реактивная мощность в щитах
        /// </summary>
        public static Guid Reaktivnaya_Moshchnost_V_SHCHitakh = new Guid("2b5a088e-af7e-4ac2-9bb8-c097b3103f2e");
        /// <summary>
        ///Пациентов
        /// </summary>
        public static Guid Patsientov = new Guid("6bac108e-d262-49fd-8097-0f7655614c83");
        /// <summary>
        ///_Пост рабочие места
        /// </summary>
        public static Guid Post_Rabochie_Mesta_ = new Guid("c157428e-0736-4580-9ecc-c087ffaffead");
        /// <summary>
        ///6_Номер документа
        /// </summary>
        public static Guid Nomer_Dokumenta6_ = new Guid("95dbb18e-7511-4f14-b3ae-30a9621e8511");
        /// <summary>
        ///Высота установки
        /// </summary>
        public static Guid Vysota_Ustanovki_1 = new Guid("5990a68f-97e6-425b-9573-8c00d3f3a6fd");
        /// <summary>
        ///RJ45
        /// </summary>
        public static Guid RJ45 = new Guid("98de1d90-d097-4fed-b78f-cf283d6c63cd");
        /// <summary>
        ///W_Аэратор_Тип
        ///Экземпляр/Текст (Параметр проекта)
        /// </summary>
        public static Guid W_Aerator_Tip = new Guid("87f27390-a19d-4ea1-9510-5d8e68849e31");
        /// <summary>
        ///Раздел проекта
        /// </summary>
        public static Guid Razdel_Proekta = new Guid("7f0c8590-53a2-4ddf-aaed-c70eb9b687bc");
        /// <summary>
        ///Частота
        /// </summary>
        public static Guid CHastota = new Guid("64d5ad90-a963-4c23-962b-b4d5597c676b");
        /// <summary>
        ///Номер зондажа
        /// </summary>
        public static Guid Nomer_Zondazha = new Guid("db7abd90-f359-489e-adea-2657695a4f79");
        /// <summary>
        ///3_Номер изменения
        /// </summary>
        public static Guid Nomer_Izmeneniya3_ = new Guid("de42db90-c96f-40cf-88e4-2e5ee730b489");
        /// <summary>
        ///Строка 5_Дата_Видимость
        /// </summary>
        public static Guid Stroka_5_Data_Vidimost = new Guid("6ba6f590-c5b2-4d18-84a5-2ad483f60bc4");
        /// <summary>
        ///Длина
        /// </summary>
        public static Guid Dlina_1 = new Guid("4c0e1d91-6536-44f1-8a93-8f93627d4c91");
        /// <summary>
        ///Ток 3КЗ
        /// </summary>
        public static Guid Tok_3KZ = new Guid("930c8891-1aa6-4ccb-a768-a728123f28e5");
        /// <summary>
        ///W_Двери_Обозначение_Автоматическая
        /// </summary>
        public static Guid W_Dveri_Oboznachenie_Avtomaticheskaya = new Guid("51d1f591-bf51-4e5a-b689-d84a549d2c1d");
        /// <summary>
        ///W_Проем_Высота
        /// </summary>
        public static Guid W_Proem_Vysota = new Guid("904c2892-744e-4d83-ae80-20f0507e9f56");
        /// <summary>
        ///W_КМ_УсилиеM
        /// </summary>
        public static Guid W_KM_UsilieM = new Guid("41455a92-6852-4eda-9454-fd7e872cc2cb");
        /// <summary>
        ///Кабельный лоток - Длина
        /// </summary>
        public static Guid Kabelnyy_Lotok__Dlina = new Guid("a3b35c92-92bd-4faf-bc22-13cd8be0f127");
        /// <summary>
        ///_Класс чистоты
        /// </summary>
        public static Guid Klass_CHistoty_ = new Guid("cf5fae92-8556-4fa1-a392-4d9958ba4408");
        /// <summary>
        ///W_Двери_Ширина основной створки
        ///Двери/Тип/Зависимости (параметр проекта)
        /// </summary>
        public static Guid W_Dveri_SHirina_Osnovnoy_Stvorki = new Guid("1bc9eb92-0a44-4ee7-a407-72bd94c8c4e6");
        /// <summary>
        ///Режим работы
        /// </summary>
        public static Guid Rezhim_Raboty = new Guid("2c559693-f06f-4edf-bdbe-84dffbe355de");
        /// <summary>
        ///Наличник
        /// </summary>
        public static Guid Nalichnik = new Guid("e3add693-3143-47a2-a053-ce7416314966");
        /// <summary>
        ///W_Перемычки_лист_a
        /// </summary>
        public static Guid W_Peremychki_List_a = new Guid("056dd993-e0d1-4048-985d-66a13d463cef");
        /// <summary>
        ///W_Окна_Маркировка проёма
        ///Окна/Экземпляр/Строительство (Параметр проекта)
        /// </summary>
        public static Guid W_Okna_Markirovka_Proema = new Guid("3e27f293-0202-45e2-a2be-df7736165c46");
        /// <summary>
        ///W_Пол_Тип сущ. отделки
        /// </summary>
        public static Guid W_Pol_Tip_Sushch_Otdelki = new Guid("d02f1594-c607-4a94-9225-3257328562f0");
        /// <summary>
        ///Габариты помещения (ВхШхД)
        /// </summary>
        public static Guid Gabarity_Pomeshcheniya_VkhSHkhD = new Guid("d9281694-c248-4c1d-b52c-51f24dc3f456");
        /// <summary>
        ///W_Марка_Правая
        /// </summary>
        public static Guid W_Marka_Pravaya = new Guid("aeed3694-bb4e-4ed2-98c4-b2b117ee0c08");
        /// <summary>
        ///WS_Стены_Площадь УТ
        ///Экземпляр/Аналитическая модель (Параметр проекта)
        /// </summary>
        public static Guid WS_Steny_Ploshchad_UT = new Guid("20f06f94-399a-4086-81a0-baa4a62c10a7");
        /// <summary>
        ///Глубина перемычки
        /// </summary>
        public static Guid Glubina_Peremychki = new Guid("1c822595-a61b-4ba6-9155-9cb1c13f730f");
        /// <summary>
        ///Маркировка типоразмера
        /// </summary>
        public static Guid Markirovka_Tiporazmera = new Guid("cbe74795-8121-4b37-a2a7-1bb05f912716");
        /// <summary>
        ///WH_Фильтрация
        ///Тип/Набор (Параметр проекта)
        /// </summary>
        public static Guid WH_Filtratsiya = new Guid("90f25295-e60a-49a0-b7a6-5e995bcc79b2");
        /// <summary>
        ///Изменение 10
        /// </summary>
        public static Guid Izmenenie_10 = new Guid("f7cfd795-b1c9-49d5-ba12-771e2f2e5797");
        /// <summary>
        ///Группа электробезопасности
        /// </summary>
        public static Guid Gruppa_Elektrobezopasnosti = new Guid("1a21e595-df34-4881-8b4e-2ec7fdd4f2ab");
        /// <summary>
        ///W_Лестницы_Отделка низа маршей_Тип
        ///Лестницы/Экземпляр/Материалы и отделка (Параметр проекта)
        /// </summary>
        public static Guid W_Lestnitsy_Otdelka_Niza_Marshey_Tip = new Guid("993d3396-fc2f-42c2-b8b1-8cb0af21a4eb");
        /// <summary>
        ///W_Жалюзи
        ///Окна/Экземпляр/Материалы и отделка (Параметр проекта)
        /// </summary>
        public static Guid W_ZHalyuzi = new Guid("193e6896-c5ab-45c5-982e-74bca550878e");
        /// <summary>
        ///Основные теплопотери
        /// </summary>
        public static Guid Osnovnye_Teplopoteri = new Guid("eae7a096-3e18-44e5-b4fb-7045fc294e83");
        /// <summary>
        ///8_Номер документа
        /// </summary>
        public static Guid Nomer_Dokumenta8_ = new Guid("eb8ee997-14b8-482d-b48d-ac6cc877dc75");
        /// <summary>
        ///W_Тип ручки
        ///Двери/Тип/Зависимости (Параметр семейства)
        /// </summary>
        public static Guid W_Tip_Ruchki = new Guid("c9925498-8911-4ab7-8c6a-59b22960cb63");
        /// <summary>
        ///WH_Сортировка вида (Назначение)
        ///Экземпляр/Набор (Параметр проекта)
        /// </summary>
        public static Guid WH_Sortirovka_Vida_Naznachenie = new Guid("82ddc298-f43d-4cc0-9592-ca717be513ac");
        /// <summary>
        ///Чистые боксы помещения
        /// </summary>
        public static Guid CHistye_Boksy_Pomeshcheniya = new Guid("ed3bcd98-424e-42db-8fa9-1183dd677cfc");
        /// <summary>
        ///W_Внешний монтажный зазор_(между коробом и кирпичными ограничителями)
        /// </summary>
        public static Guid W_Vneshniy_Montazhnyy_Zazor_Mezhdu_Korobom_I_Kirpichnymi_Ogranichitelyami = new Guid("6819ee98-0fd0-4997-914f-88d6e27f64b4");
        /// <summary>
        ///Наименование, габариты, смежные разделы
        /// </summary>
        public static Guid Naimenovanie_Gabarity_Smezhnye_Razdely = new Guid("aca80499-a356-47e7-944c-7f853d724196");
        /// <summary>
        ///Наименование объекта
        /// </summary>
        public static Guid Naimenovanie_Obekta = new Guid("c92f2899-a807-468d-8126-610f73aa6ffc");
        /// <summary>
        ///W_Лестницы_Марка
        /// </summary>
        public static Guid W_Lestnitsy_Marka = new Guid("f0384a99-b373-4857-acb2-69012e610467");
        /// <summary>
        ///W_Ширина бруса короба_(обсады)
        /// </summary>
        public static Guid W_SHirina_Brusa_Koroba_Obsady = new Guid("0bf76599-a512-40a2-8fdc-81fb473b6df3");
        /// <summary>
        ///Шифр_Экспертиза
        /// </summary>
        public static Guid SHifr_Ekspertiza = new Guid("bd839599-ee39-4a5e-9b64-597b933792b5");
        /// <summary>
        ///Направление
        /// </summary>
        public static Guid Napravlenie = new Guid("f44cbd99-e743-4e52-86ae-6435f9e96a13");
        /// <summary>
        ///ID крепления
        /// </summary>
        public static Guid ID_Krepleniya = new Guid("bceddf99-3936-483d-8ecf-b0956b490597");
        /// <summary>
        ///ADSK_Позиция
        ///Позиция элемента модели, которая выносится в марку элемента на планы и отображается в спецификациях
        /// </summary>
        public static Guid ADSK_Pozitsiya = new Guid("ae8ff999-1f22-4ed7-ad33-61503d85f0f4");
        /// <summary>
        ///6_Дата
        /// </summary>
        public static Guid Data6_ = new Guid("d974d29a-ef2d-46fb-a350-e0122df84727");
        /// <summary>
        ///Классификация изображения нагрузки для схем
        /// </summary>
        public static Guid Klassifikatsiya_Izobrazheniya_Nagruzki_Dlya_Skhem = new Guid("ffef279b-4e26-4115-a0a0-8045f5824a5b");
        /// <summary>
        ///Кол-во изделий №5
        /// </summary>
        public static Guid Kolvo_Izdeliy_5 = new Guid("a83b6f9b-26c0-4037-ab65-2a68fb216da4");
        /// <summary>
        ///ЖБ отделка
        /// </summary>
        public static Guid ZHB_Otdelka = new Guid("da5eed9b-cce0-43c6-9d03-57f90e342712");
        /// <summary>
        ///ADSK_Марка
        ///Тип, марка, обозначение документа, опросного листа
        /// </summary>
        public static Guid ADSK_Marka = new Guid("2204049c-d557-4dfc-8d70-13f19715e46d");
        /// <summary>
        ///W_Двери_Примечания_Влагостойкость
        /// </summary>
        public static Guid W_Dveri_Primechaniya_Vlagostoykost = new Guid("5ef30a9c-236f-4cce-9d2f-759a02f273e8");
        /// <summary>
        ///W_Сечение_Тип
        /// </summary>
        public static Guid W_Sechenie_Tip = new Guid("880d699c-6751-4053-bfd1-a6802d05e1de");
        /// <summary>
        ///01_Объем
        ///Объем
        /// </summary>
        public static Guid Obem01_ = new Guid("50bfa99c-8ab5-4e84-ab7b-ffe0c8606ad6");
        /// <summary>
        ///W_Ширина бокового внутреннего откоса
        /// </summary>
        public static Guid W_SHirina_Bokovogo_Vnutrennego_Otkosa = new Guid("dc2e0b9d-f882-4292-8ba0-d397939583b6");
        /// <summary>
        ///Расход материалов
        /// </summary>
        public static Guid Raskhod_Materialov = new Guid("aaed9e9d-d7eb-4431-9cf8-4dc74f90a654");
        /// <summary>
        ///Обработка инструментов
        /// </summary>
        public static Guid Obrabotka_Instrumentov = new Guid("f3a1b19d-e49b-4d17-976c-cffd67208f88");
        /// <summary>
        ///W_Дополнительные элементы
        ///Двери/Тип/Зависимости (Параметр проекта)
        /// </summary>
        public static Guid W_Dopolnitelnye_Elementy = new Guid("5413329e-9d7a-4690-b96f-9feea04a3a79");
        /// <summary>
        ///WS_Потолок_Номера помещений по типу
        /// </summary>
        public static Guid WS_Potolok_Nomera_Pomeshcheniy_Po_Tipu = new Guid("da78a09e-c128-4104-b694-35dabe97768c");
        /// <summary>
        ///Расчет ПБ
        /// </summary>
        public static Guid Raschet_PB = new Guid("399cf39e-1c89-4c7b-a743-7af78b3ff28f");
        /// <summary>
        ///W_Отметки остановок
        ///Оборудование/Экземпляр/Зависимости (Параметр семейства)
        /// </summary>
        public static Guid W_Otmetki_Ostanovok = new Guid("890a2e9f-3f02-461e-a39b-45e3e14fe460");
        /// <summary>
        ///7_Номер документа
        /// </summary>
        public static Guid Nomer_Dokumenta7_ = new Guid("af79ae9f-57ab-4ec6-b8bc-c1d7650388df");
        /// <summary>
        ///WS_Усиление_Высота
        ///Двери, окна/Экземпляр/Текст (Параметр проекта)
        /// </summary>
        public static Guid WS_Usilenie_Vysota = new Guid("8f12df9f-b467-40bf-abe1-55c4dfedba5d");
        /// <summary>
        ///_Температура притока
        /// </summary>
        public static Guid Temperatura_Pritoka_ = new Guid("d72a4ba0-9f30-45f5-a122-1b4f43cd1ad6");
        /// <summary>
        ///Строка 4_Дата_Видимость
        /// </summary>
        public static Guid Stroka_4_Data_Vidimost = new Guid("22fd7da0-60c5-4e4f-88b2-6f0103b59b6b");
        /// <summary>
        ///Единицы измерения
        /// </summary>
        public static Guid Edinitsy_Izmereniya = new Guid("7f32d7a0-8f48-4525-900f-2352f4b00bd6");
        /// <summary>
        ///WS_Стены_Площадь отделки з.п.
        ///Экземпляр/Аналитическая модель (Параметр проекта)
        /// </summary>
        public static Guid WS_Steny_Ploshchad_Otdelki_Zp = new Guid("a30b35a1-6a97-4348-a085-06602fcaf98c");
        /// <summary>
        ///Аксессуары лотка
        /// </summary>
        public static Guid Aksessuary_Lotka = new Guid("e7aa4aa1-6b4b-4675-92c6-64cc807b4a6f");
        /// <summary>
        ///Класс пожарной опасности
        /// </summary>
        public static Guid Klass_Pozharnoy_Opasnosti = new Guid("1f9976a1-804c-49f6-b40f-16c0a2a25227");
        /// <summary>
        ///Комментарии к типоразмеру
        /// </summary>
        public static Guid Kommentarii_K_Tiporazmeru = new Guid("3a78d3a1-d677-4bb2-8d5f-0947954af904");
        /// <summary>
        ///Сопротивление контактов гр. щитов (Zпк)
        /// </summary>
        public static Guid Soprotivlenie_Kontaktov_Gr_SHCHitov_Zpk = new Guid("a405f1a1-4105-4924-ad33-d73c3aac7b81");
        /// <summary>
        ///Прочее
        /// </summary>
        public static Guid Prochee = new Guid("655effa1-1c78-4922-a005-a213290e82e7");
        /// <summary>
        ///WS_Усиление
        ///Двери, окна/Экземпляр/Текст (Параметр проекта)
        /// </summary>
        public static Guid WS_Usilenie = new Guid("3576a3a2-1c25-48c6-9cb5-a51aafdb00b8");
        /// <summary>
        ///W_Проем_Глубина
        /// </summary>
        public static Guid W_Proem_Glubina = new Guid("1a1207a3-fcc8-4d97-9fab-cf76bb30cd2e");
        /// <summary>
        ///Вид сбора отходов
        /// </summary>
        public static Guid Vid_Sbora_Otkhodov = new Guid("3da320a3-0f34-4ff3-8b54-a782bc70b1bc");
        /// <summary>
        ///W_Двери_Обозначение_Взломоустойчивая
        /// </summary>
        public static Guid W_Dveri_Oboznachenie_Vzlomoustoychivaya = new Guid("3ae959a3-dc03-4459-a549-d040a5631e54");
        /// <summary>
        ///Код совестимости
        /// </summary>
        public static Guid Kod_Sovestimosti = new Guid("14f7e2a3-0082-4b59-a78c-b060f171b8cf");
        /// <summary>
        ///Описание по классификатору
        /// </summary>
        public static Guid Opisanie_Po_Klassifikatoru = new Guid("77f5eba3-8d9e-46fb-b295-e8f6192bea97");
        /// <summary>
        ///Наличие влажных процессов
        /// </summary>
        public static Guid Nalichie_Vlazhnykh_Protsessov = new Guid("badfcba4-6bb1-4f0c-a7a6-2fa6300bbc11");
        /// <summary>
        ///Производитель 2
        /// </summary>
        public static Guid Proizvoditel_2 = new Guid("722fe5a4-4e3a-464a-9e2c-07e75ecc3c5a");
        /// <summary>
        ///ADSK_Группирование
        ///Параметр для пользовательского группирования элементов в спецификациях
        /// </summary>
        public static Guid ADSK_Gruppirovanie = new Guid("3de5f1a4-d560-4fa8-a74f-25d250fb3401");
        /// <summary>
        ///Производитель
        /// </summary>
        public static Guid Proizvoditel = new Guid("21de13a5-06d1-4ad5-81d5-50a098035ee7");
        /// <summary>
        ///Крепеж
        /// </summary>
        public static Guid Krepezh = new Guid("f5172fa5-d91e-425a-92b6-eacf0137c68d");
        /// <summary>
        ///Ось Л
        /// </summary>
        public static Guid Os_L = new Guid("1a2f6da5-c889-4f1a-80c2-4da6063d501c");
        /// <summary>
        ///Число листов
        /// </summary>
        public static Guid CHislo_Listov = new Guid("c3b69ca5-3c62-48cb-95b4-98f06c46b9fa");
        /// <summary>
        ///W_ОбъемНизкойДетализацииВкл
        /// </summary>
        public static Guid W_ObemNizkoyDetalizatsiiVkl = new Guid("64b1a6a5-1f28-40e8-bb2d-e6e4da8e94e4");
        /// <summary>
        ///W_Номер типа
        ///Тип/Тескт (Параметр проекта)
        /// </summary>
        public static Guid W_Nomer_Tipa = new Guid("573ae8a5-d8ed-421e-8771-8aacb46254c9");
        /// <summary>
        ///Декоративные замковые камни
        /// </summary>
        public static Guid Dekorativnye_Zamkovye_Kamni = new Guid("c52633a6-53c4-4951-bda5-1d30855387db");
        /// <summary>
        ///Крепление к потолку
        /// </summary>
        public static Guid Kreplenie_K_Potolku = new Guid("61a8d8a6-63fe-482c-bb3c-e284808955de");
        /// <summary>
        ///Минимальное рабочее напряжение
        /// </summary>
        public static Guid Minimalnoe_Rabochee_Napryazhenie = new Guid("c23df7a6-3f6f-416f-9f35-5cdf455fd6d5");
        /// <summary>
        ///W_Двери_Обозначение_Раздвижная/Складная
        /// </summary>
        public static Guid W_Dveri_Oboznachenie_RazdvizhnayaSkladnaya = new Guid("aa9dfca6-ee72-4af6-8801-cad29411ec32");
        /// <summary>
        ///10_Количество участков
        /// </summary>
        public static Guid Kolichestvo_Uchastkov10_ = new Guid("47d820a7-49b7-46a9-928c-4f6ca87520b1");
        /// <summary>
        ///Габаритная ширина ниши
        /// </summary>
        public static Guid Gabaritnaya_SHirina_Nishi = new Guid("3035b1a7-2beb-4538-9e7b-2af2c71a5604");
        /// <summary>
        ///WS_ВОР_Единица измерения_Тип
        ///Все категории/Тип/Прочее (Параметр проекта)
        /// </summary>
        public static Guid WS_VOR_Edinitsa_Izmereniya_Tip = new Guid("51f71ba8-f5ce-4bb0-8b64-ce50ba9e2cdf");
        /// <summary>
        ///Остановка на этажах
        /// </summary>
        public static Guid Ostanovka_Na_Etazhakh = new Guid("e24c59a8-420e-4ef1-8d83-5ecfbf17bb32");
        /// <summary>
        ///7_Номер изменения
        /// </summary>
        public static Guid Nomer_Izmeneniya7_ = new Guid("8863f0a8-6d74-4caf-9cb8-1b60ee35b0ba");
        /// <summary>
        ///Номер листа (вручную)
        /// </summary>
        public static Guid Nomer_Lista_Vruchnuyu_1 = new Guid("7a8d26a9-3b3c-45d5-94a8-fdc9474c35b0");
        /// <summary>
        ///Масса изд. №6
        /// </summary>
        public static Guid Massa_Izd_6 = new Guid("d589c9a9-5338-4d42-93bc-12b8dd1a87e0");
        /// <summary>
        ///W_Свинцовый эквивалент (Pb)
        ///Стены, двери, окна/Экземпляр/Набор (Семейство проекта)
        /// </summary>
        public static Guid W_Svintsovyy_Ekvivalent_Pb = new Guid("6686e2a9-4641-46f2-bfa7-54afc249f850");
        /// <summary>
        ///Угол изделия для спецификации
        /// </summary>
        public static Guid Ugol_Izdeliya_Dlya_Spetsifikatsii = new Guid("112fe4a9-d891-4ed9-bc66-cbdc99d218cf");
        /// <summary>
        ///W_Шифр проекта_Стадия Р
        ///Листы/Текст (Общий параметр)
        /// </summary>
        public static Guid W_SHifr_Proekta_Stadiya_R_1 = new Guid("16afeea9-a157-4c9c-9f48-529514c49296");
        /// <summary>
        ///7_Дата
        /// </summary>
        public static Guid Data7_ = new Guid("99d924aa-a817-4e8f-8f1c-347eaf8488a3");
        /// <summary>
        ///К Одноврем. Работы 0.5..1
        /// </summary>
        public static Guid K_Odnovrem_Raboty_051 = new Guid("372330aa-928a-432b-93b8-bb9c38743b74");
        /// <summary>
        ///WS_Площадь пола
        /// </summary>
        public static Guid WS_Ploshchad_Pola = new Guid("5dcc72aa-a989-4b9b-8da7-09a72ca7e9f7");
        /// <summary>
        ///Коэффициент одновременности
        /// </summary>
        public static Guid Koeffitsient_Odnovremennosti = new Guid("f4ed40ab-f5fa-483a-b059-eec28573928e");
        /// <summary>
        ///Материал
        /// </summary>
        public static Guid Material = new Guid("8180a5ab-983b-49af-bbf3-3c98fc767b29");
        /// <summary>
        ///7_Количество участков
        /// </summary>
        public static Guid Kolichestvo_Uchastkov7_ = new Guid("8f0241ac-88f5-4fbb-ae8f-7f69fa21e2d8");
        /// <summary>
        ///Не_пересекается
        /// </summary>
        public static Guid Ne_Peresekaetsya = new Guid("95fba7ac-8b53-4c24-8f4c-f90f8fedfb08");
        /// <summary>
        ///Ширина проёма
        /// </summary>
        public static Guid SHirina_Proema = new Guid("8dd3efac-2b8d-4acd-9cda-f9e48ce544d6");
        /// <summary>
        ///Строка 7 должность
        /// </summary>
        public static Guid Stroka_7_Dolzhnost = new Guid("6f670cad-cc0d-411f-9ed7-762928dbb886");
        /// <summary>
        ///_Расчетное тепловыделение
        /// </summary>
        public static Guid Raschetnoe_Teplovydelenie_ = new Guid("6dc41cad-6ecf-4bd2-aa00-1f931b4fad01");
        /// <summary>
        ///Масса изд. №1 (п.м.)
        /// </summary>
        public static Guid Massa_Izd_1_Pm = new Guid("9cd369ad-d1ab-474a-99fd-7872f985a3f9");
        /// <summary>
        ///WH_Дизайн-проект
        ///Помещения/Экземпляр/Строительство (Параметр проекта)
        /// </summary>
        public static Guid WH_Dizaynproekt = new Guid("3a416fae-ec3a-4ec5-9aec-1edfcef7613f");
        /// <summary>
        ///СВ8
        /// </summary>
        public static Guid SV8 = new Guid("f95d95ae-c13b-4789-a824-eaf7f9a0b28a");
        /// <summary>
        ///Высота внешнего проёма (отчёт)
        /// </summary>
        public static Guid Vysota_Vneshnego_Proema_Otchet = new Guid("a2f4b1ae-700d-42f9-9eaa-79644589a218");
        /// <summary>
        ///TV
        /// </summary>
        public static Guid TV = new Guid("9a54b8ae-be3c-43d5-9dc7-b65fe05d59f6");
        /// <summary>
        ///а
        /// </summary>
        public static Guid a = new Guid("67d2caaf-897d-4808-b701-2032de526602");
        /// <summary>
        ///1_Номер документа
        /// </summary>
        public static Guid Nomer_Dokumenta1_ = new Guid("42aad4af-8f19-4d83-9444-fb42a6146953");
        /// <summary>
        ///Высота конструкции
        /// </summary>
        public static Guid Vysota_Konstruktsii = new Guid("75c729b0-8c49-4d79-b518-c19cd29b2e42");
        /// <summary>
        ///2_Лист
        /// </summary>
        public static Guid List2_ = new Guid("d2f73cb0-746a-45f7-b829-7edcbc38be5b");
        /// <summary>
        ///W_Откосы_Вн_У заполнения_Высота
        /// </summary>
        public static Guid W_Otkosy_Vn_U_Zapolneniya_Vysota = new Guid("ff8244b0-66b1-4807-bd3f-5fe4929d024c");
        /// <summary>
        ///Углекислый газ
        /// </summary>
        public static Guid Uglekislyy_Gaz = new Guid("7a2a72b0-a70a-447a-894a-4765c2381e7c");
        /// <summary>
        ///W_Грузоподъемность
        ///Оборудование/Тип/Механизмы (Параметр семейства)
        /// </summary>
        public static Guid W_Gruzopodemnost = new Guid("c218b4b0-bce7-448b-866f-732a26982431");
        /// <summary>
        ///WS_Стены_Площадь ГК
        ///Экземпляр/Аналитическая модель (Параметр проекта)
        /// </summary>
        public static Guid WS_Steny_Ploshchad_GK = new Guid("e82fbfb0-8934-4efb-90c7-34ad88a5213c");
        /// <summary>
        ///Максимальное количество выводов
        /// </summary>
        public static Guid Maksimalnoe_Kolichestvo_Vyvodov = new Guid("9145f4b0-6f66-446b-ae9e-d627d84aa01e");
        /// <summary>
        ///WS_Стены_Площадь К2
        ///Экземпляр/Аналитическая модель (Параметр проекта)
        /// </summary>
        public static Guid WS_Steny_Ploshchad_K2 = new Guid("80aabfb1-5e41-439c-9ac7-3300e02c5869");
        /// <summary>
        ///Группа медицинских помещений по ГОСТ Р 50571.28-2006
        /// </summary>
        public static Guid Gruppa_Meditsinskikh_Pomeshcheniy_Po_GOST_R_50571282006 = new Guid("1808c2b1-0460-40eb-a300-49b73ad2896d");
        /// <summary>
        ///Санитаров
        /// </summary>
        public static Guid Sanitarov = new Guid("451eddb1-ccc8-492e-9000-418873f4fdae");
        /// <summary>
        ///Х.в.
        /// </summary>
        public static Guid KHv = new Guid("f9b2eab1-9936-48e8-a63c-1c931c5d3809");
        /// <summary>
        ///Описание для спецификации
        /// </summary>
        public static Guid Opisanie_Dlya_Spetsifikatsii = new Guid("8e94ffb1-7646-4f59-942b-e4bc5e1be27b");
        /// <summary>
        ///Размер фитинга вложенного семейства
        ///Для группирования по фактическому размеру соединения (для выявления отсутствующих у производителя типоразмеров фитингов)
        /// </summary>
        public static Guid Razmer_Fitinga_Vlozhennogo_Semeystva = new Guid("78d84fb2-b29f-41d2-b993-b4e5cd7ecaa0");
        /// <summary>
        ///Категория надежности
        /// </summary>
        public static Guid Kategoriya_Nadezhnosti = new Guid("613196b2-ac47-40a0-9f66-1df49be78718");
        /// <summary>
        ///WS_Площадь потолка
        /// </summary>
        public static Guid WS_Ploshchad_Potolka = new Guid("f883b1b2-ffe5-4547-ae8c-963aac2f5cde");
        /// <summary>
        ///Задание АОВ
        /// </summary>
        public static Guid Zadanie_AOV = new Guid("4275f5b2-7c5e-453b-87b7-68d2ea7b801d");
        /// <summary>
        ///W_Двери_Перемычка
        /// </summary>
        public static Guid W_Dveri_Peremychka = new Guid("d52f05b3-f6a6-4036-8b3a-c9f6fd8ec676");
        /// <summary>
        ///Влажность
        /// </summary>
        public static Guid Vlazhnost = new Guid("895b1bb3-0dbd-4d8c-b171-f917379cd1b0");
        /// <summary>
        ///W_Двери_Вес(кг)
        /// </summary>
        public static Guid W_Dveri_Veskg = new Guid("29ef1eb3-c1e3-443a-bfce-9cf358941946");
        /// <summary>
        ///Схема пола
        /// </summary>
        public static Guid Skhema_Pola = new Guid("342a2bb3-780c-4a96-be0e-a1ebc1f88292");
        /// <summary>
        ///4_Количество участков
        /// </summary>
        public static Guid Kolichestvo_Uchastkov4_ = new Guid("91d72cb3-c0c6-4f12-a853-e7c0997d7468");
        /// <summary>
        ///W_Трещины_Раскрытие
        /// </summary>
        public static Guid W_Treshchiny_Raskrytie = new Guid("0677aab3-e990-4a96-bde2-f8f7c82b187a");
        /// <summary>
        ///W_Откосы_Нар_У стены_Высота до дуги
        /// </summary>
        public static Guid W_Otkosy_Nar_U_Steny_Vysota_Do_Dugi = new Guid("f9c2c9b3-56be-474d-91d6-79a60409811a");
        /// <summary>
        ///ГБ отделка з.п.
        /// </summary>
        public static Guid GB_Otdelka_Zp = new Guid("eeae30b4-2180-4e1b-93b3-6e65b6c24156");
        /// <summary>
        ///Строка 8 фамилия
        /// </summary>
        public static Guid Stroka_8_Familiya = new Guid("113ed0b4-7339-4206-a9af-c4148fb2da5e");
        /// <summary>
        ///Марка кабеля в щитах
        /// </summary>
        public static Guid Marka_Kabelya_V_SHCHitakh = new Guid("56b0eab4-b53a-4793-81c0-9349c03009a5");
        /// <summary>
        ///WSH_Фахверк_Вес 1 м.п.
        ///Стены/Тип/Несущие конструкции (Параметр проекта)
        /// </summary>
        public static Guid WSH_Fakhverk_Ves_1_Mp = new Guid("6102f3b4-ed08-47f2-9869-17eccf8a16e1");
        /// <summary>
        ///Номер группы в щитах
        /// </summary>
        public static Guid Nomer_Gruppy_V_SHCHitakh = new Guid("391722b5-efb8-409a-a5cf-710a81dfa07a");
        /// <summary>
        ///W_Откосы_Площадь
        ///Окна/Экземпляр/Размеры (Параметр семейства)
        /// </summary>
        public static Guid W_Otkosy_Ploshchad = new Guid("45b570b5-0f99-433b-937a-8a898b379209");
        /// <summary>
        ///Ось Л помещения
        /// </summary>
        public static Guid Os_L_Pomeshcheniya = new Guid("b85f8bb5-6e9a-47a0-8354-431a755297af");
        /// <summary>
        ///WS_Описание отделки по К2
        ///Помещения/Экземпляр/Аналитическая модель (Параметр проекта)
        /// </summary>
        public static Guid WS_Opisanie_Otdelki_Po_K2 = new Guid("5ee6a2b5-5c90-43c9-a6e6-4d3688af302e");
        /// <summary>
        ///Крепление к полу
        /// </summary>
        public static Guid Kreplenie_K_Polu = new Guid("4edeafb5-e0ea-4c27-b7ed-397ec049d8f4");
        /// <summary>
        ///W_Высота
        /// </summary>
        public static Guid W_Vysota = new Guid("fb8efdb5-30c8-49b9-8352-1376c643c201");
        /// <summary>
        ///Площадь нормативная
        /// </summary>
        public static Guid Ploshchad_Normativnaya = new Guid("ec0413b6-b0d1-457a-8f9b-836acbbc2d1a");
        /// <summary>
        ///W_Кровля_Конструкция
        ///Крыши/Тип/Идентификация (Параметр проекта)
        /// </summary>
        public static Guid W_Krovlya_Konstruktsiya = new Guid("bf0355b6-635f-49c1-bc1e-16a688fe0c80");
        /// <summary>
        ///a - Ширина
        /// </summary>
        public static Guid a__SHirina = new Guid("a6e85db6-25dd-4cda-a78e-4f0ed673955d");
        /// <summary>
        ///Этаж_Целое
        /// </summary>
        public static Guid Etazh_TSeloe = new Guid("e01967b6-d3b9-4b8f-b69c-d47625ba3c68");
        /// <summary>
        ///WS_Площадь потолка_м.кв
        /// </summary>
        public static Guid WS_Ploshchad_Potolka_Mkv = new Guid("bb5e77b6-fe96-4cb4-8e49-8c2b124102a2");
        /// <summary>
        ///WSH_Наличие в классификаторе
        ///Помещения/Экземпляр/Прочее (Параметр проекта)
        /// </summary>
        public static Guid WSH_Nalichie_V_Klassifikatore = new Guid("68a0d2b6-ed9e-4ca1-8f26-5c2f2c0ce857");
        /// <summary>
        ///W_Пол_Состав
        ///Экземпляр/Строительство (Параметр проекта)
        /// </summary>
        public static Guid W_Pol_Sostav = new Guid("d73f85b8-74c1-4624-810b-d3484e1ba8a4");
        /// <summary>
        ///W_Глубина внутреннего откоса_(от внутренней штукатурки до кирпичного перепада)
        /// </summary>
        public static Guid W_Glubina_Vnutrennego_Otkosa_Ot_Vnutrenney_SHtukaturki_Do_Kirpichnogo_Perepada = new Guid("b8e1c3b8-fec7-411c-8290-12094841f7b5");
        /// <summary>
        ///Масса изд. №1
        /// </summary>
        public static Guid Massa_Izd_1 = new Guid("b96ae3b8-f51d-4c5b-b72e-e2d0f0ffb58e");
        /// <summary>
        ///WS_Описание отделки по ГБ
        ///Помещения/Экземпляр/Аналитическая модель (Параметр проекта)
        /// </summary>
        public static Guid WS_Opisanie_Otdelki_Po_GB = new Guid("d8c620b9-3bd9-408c-a97f-d179d0257b04");
        /// <summary>
        ///Длина цепи со смещением
        /// </summary>
        public static Guid Dlina_TSepi_So_Smeshcheniem = new Guid("48cb22b9-5f47-4d45-978d-4a5749462053");
        /// <summary>
        ///Пребывание персонала
        /// </summary>
        public static Guid Prebyvanie_Personala = new Guid("d7282eb9-2516-48bd-818e-17568bc3cbcd");
        /// <summary>
        ///8_Дата
        /// </summary>
        public static Guid Data8_ = new Guid("b1e2a3b9-e98c-4835-988a-398fa4062f03");
        /// <summary>
        ///Ток ОКЗ
        /// </summary>
        public static Guid Tok_OKZ = new Guid("a8dc10ba-23c7-49f7-b987-d8f2493116e9");
        /// <summary>
        ///Отделка пола 2 (описание)
        /// </summary>
        public static Guid Otdelka_Pola_2_Opisanie = new Guid("d29889ba-1786-41ca-92f0-dffff816cf8b");
        /// <summary>
        ///Общее реактивное сопротивление, мОм
        /// </summary>
        public static Guid Obshchee_Reaktivnoe_Soprotivlenie_MOm = new Guid("f7dfafba-c368-43c1-b37d-4e1b8b5ebb3f");
        /// <summary>
        ///W_Щеколда изнутри
        ///Двери/Тип/Зависимости (Параметр семейства)
        /// </summary>
        public static Guid W_SHCHekolda_Iznutri = new Guid("adc7d7ba-96c9-4e69-90f5-038fff057703");
        /// <summary>
        ///1_Количество участков
        /// </summary>
        public static Guid Kolichestvo_Uchastkov1_ = new Guid("4006d1bb-10c7-46cc-bb0a-cc17b64cd1b4");
        /// <summary>
        ///Строка 3 должность
        /// </summary>
        public static Guid Stroka_3_Dolzhnost = new Guid("3dc943bc-1b11-47e6-9cde-880d51a9d861");
        /// <summary>
        ///W_Двери_Примечание_СКУД
        ///Система контроля и управление доступом&#xD&#xA
        /// </summary>
        public static Guid W_Dveri_Primechanie_SKUD = new Guid("cd0950bc-de81-4cce-a0e3-2d3fcabb6d00");
        /// <summary>
        ///W_Внешняя ширина откоса на плоскости стены_(основание-кирпич)
        /// </summary>
        public static Guid W_Vneshnyaya_SHirina_Otkosa_Na_Ploskosti_Steny_Osnovaniekirpich = new Guid("ec408dbc-1a90-4aed-8ff6-0a8f01cf7d32");
        /// <summary>
        ///X источника
        /// </summary>
        public static Guid X_Istochnika = new Guid("4b3890bc-8304-45e6-91fa-a5214be59667");
        /// <summary>
        ///Тип прибора
        /// </summary>
        public static Guid Tip_Pribora = new Guid("d9da97bc-8c1d-400f-8149-4212d57c313e");
        /// <summary>
        ///Штамп
        /// </summary>
        public static Guid SHtamp = new Guid("5143a2bc-0eab-48df-971a-534aa54e62b5");
        /// <summary>
        ///Элементы пола и их толщина
        /// </summary>
        public static Guid Elementy_Pola_I_Ikh_Tolshchina = new Guid("3c63aebc-e107-4778-8c0a-6d47db1e0a95");
        /// <summary>
        ///W_Двери_Обозначение_Группа по месту установки
        ///А (наружные)/ В (внутр., между отаплив. пом.я внутри зданий, в том числе общественных (офисы, кабинеты), а также на путях эвакуации)/ В1 (внутр. для вспом.пом-й)/ Г (наружные с усиленными функциями)
        /// </summary>
        public static Guid W_Dveri_Oboznachenie_Gruppa_Po_Mestu_Ustanovki = new Guid("4f39fdbc-7343-412d-b7b5-808931346d0e");
        /// <summary>
        ///WH_Символ
        /// </summary>
        public static Guid WH_Simvol = new Guid("73fe65bd-80f2-4e1a-b8d1-1444944d2bdb");
        /// <summary>
        ///ADSK_Номер стояка
        ///Обозначение стояка на схемах, планах
        /// </summary>
        public static Guid ADSK_Nomer_Stoyaka = new Guid("77c39bbd-2b91-4602-92a8-cf4e1e3b62c1");
        /// <summary>
        ///Вес нетто
        /// </summary>
        public static Guid Ves_Netto = new Guid("32da9dbd-af6d-4193-a80e-d343c60409a7");
        /// <summary>
        ///Функциональное отделение помещения
        /// </summary>
        public static Guid Funktsionalnoe_Otdelenie_Pomeshcheniya = new Guid("58d887be-f9ed-4e83-bdd9-85c8d9486923");
        /// <summary>
        ///Высота подвесного потолка
        /// </summary>
        public static Guid Vysota_Podvesnogo_Potolka = new Guid("4ba00dbf-5fdf-4026-87fb-1a092021b6c2");
        /// <summary>
        ///W_Лестницы_Отделка плинтуса_Описание
        ///Лестницы/Экземпляр/Материалы и отделка (Параметр проекта)
        /// </summary>
        public static Guid W_Lestnitsy_Otdelka_Plintusa_Opisanie = new Guid("189481bf-a31a-4f54-b8e5-fca0275d3da1");
        /// <summary>
        ///WS_Площадь пола_м.кв
        /// </summary>
        public static Guid WS_Ploshchad_Pola_Mkv = new Guid("0394b4bf-35e8-4fff-89f0-4445d159a838");
        /// <summary>
        ///W_Длина балки аналитическая
        /// </summary>
        public static Guid W_Dlina_Balki_Analiticheskaya = new Guid("942551c0-d643-4c80-b8c7-95b170ad43ef");
        /// <summary>
        ///УТ площадь з.п.
        /// </summary>
        public static Guid UT_Ploshchad_Zp = new Guid("7e92ccc0-f0bb-4cc9-8595-43cd9bf9a9d2");
        /// <summary>
        ///3_Количество участков
        /// </summary>
        public static Guid Kolichestvo_Uchastkov3_ = new Guid("d679e6c1-2814-4dd7-b5fc-98bcb6946a11");
        /// <summary>
        ///Проектная стадия
        /// </summary>
        public static Guid Proektnaya_Stadiya = new Guid("9c27edc1-ca3c-4073-8844-1bd59e5e2d94");
        /// <summary>
        ///Крепление к стене
        /// </summary>
        public static Guid Kreplenie_K_Stene = new Guid("391bc7c2-d4f5-4ea9-8370-95c4c82f7477");
        /// <summary>
        ///Kуд коэффициент ударного тока
        /// </summary>
        public static Guid Kud_Koeffitsient_Udarnogo_Toka = new Guid("ab42edc2-994e-44ee-b940-c60ee16c9898");
        /// <summary>
        ///Чиста
        /// </summary>
        public static Guid CHista = new Guid("c1f230c3-10d9-4ff4-a052-180b840db9c7");
        /// <summary>
        ///W_Лестницы_Отделка плинтуса_Площадь
        ///Лестницы/Экземпляр/Материалы и отделка (Параметр проекта)
        /// </summary>
        public static Guid W_Lestnitsy_Otdelka_Plintusa_Ploshchad = new Guid("523456c3-53a2-411e-bea1-10d6219fcaff");
        /// <summary>
        ///Описание
        /// </summary>
        public static Guid Opisanie = new Guid("206d61c3-f8d3-4d61-9e90-cc491a37fa27");
        /// <summary>
        ///W_Доступ МГН
        /// </summary>
        public static Guid W_Dostup_MGN = new Guid("d93a79c3-dae5-4713-8abb-badadb997dec");
        /// <summary>
        ///Площадь реальная
        /// </summary>
        public static Guid Ploshchad_Realnaya = new Guid("85737ec3-1afc-425b-bc00-3959a09dc3df");
        /// <summary>
        ///W_Стены_Тип сущ. отделки
        /// </summary>
        public static Guid W_Steny_Tip_Sushch_Otdelki = new Guid("4908dcc3-b103-41f2-a222-242c56927a7e");
        /// <summary>
        ///К2 площадь з.п.
        /// </summary>
        public static Guid K2_Ploshchad_Zp = new Guid("b15ed7c4-cf76-4fe2-a0a8-b7903b9b9a07");
        /// <summary>
        ///W_Откосы_Площадь наруж.
        /// </summary>
        public static Guid W_Otkosy_Ploshchad_Naruzh = new Guid("6875fdc4-dc06-4856-9336-5af4ddb45fd4");
        /// <summary>
        ///W_Двери_Места установки
        /// </summary>
        public static Guid W_Dveri_Mesta_Ustanovki = new Guid("1f955ac5-5a69-4078-9555-a0325d37d9b4");
        /// <summary>
        ///W_Перемычки_Позиция_2
        /// </summary>
        public static Guid W_Peremychki_Pozitsiya_2 = new Guid("8439d2c5-b48d-4eb4-b7fe-403cf3d063c5");
        /// <summary>
        ///Интервал крепления к полу
        /// </summary>
        public static Guid Interval_Krepleniya_K_Polu = new Guid("291320c6-cf84-4503-9600-10a4397e94fc");
        /// <summary>
        ///3_Дата
        /// </summary>
        public static Guid Data3_ = new Guid("07b0bcc6-ecfc-4b93-b8c3-5eb6e25b848a");
        /// <summary>
        ///Тангенс в щитах
        /// </summary>
        public static Guid Tangens_V_SHCHitakh = new Guid("b8503fc7-e2ab-4b62-905b-5fe15b311e93");
        /// <summary>
        ///W_Накладки
        ///Двери/Тип/Зависимости (Параметр семейства)
        /// </summary>
        public static Guid W_Nakladki = new Guid("1d51a0c7-2e3f-4636-b3b0-69ee83b08f1a");
        /// <summary>
        ///W_Отделение
        ///Помещения/Экземпляр/Текст (Параметр проекта)
        /// </summary>
        public static Guid W_Otdelenie = new Guid("bd77e2c7-621f-48c7-8cf8-ef626bfcc945");
        /// <summary>
        ///Количество светильников
        /// </summary>
        public static Guid Kolichestvo_Svetilnikov = new Guid("805e1dc8-6c5d-4abe-a926-d1122bf5b094");
        /// <summary>
        ///W_Створки_Описание
        /// </summary>
        public static Guid W_Stvorki_Opisanie = new Guid("f5147dc8-3136-4f14-9d37-689cb6f8e088");
        /// <summary>
        ///Фартук
        /// </summary>
        public static Guid Fartuk = new Guid("e45e81c8-1582-4795-9e0f-b79aeb4ef251");
        /// <summary>
        ///W_Потолок_Высота
        ///Экземпляр/Набор (Параметр проекта)
        /// </summary>
        public static Guid W_Potolok_Vysota = new Guid("0f90abc8-6a40-462f-95b7-4ed268c953cf");
        /// <summary>
        ///Способ прокладки кабелей для ОС
        /// </summary>
        public static Guid Sposob_Prokladki_Kabeley_Dlya_OS = new Guid("914fd7c8-80ed-4e93-9461-13e8c8fec57d");
        /// <summary>
        ///ЖБ отделка з.п.
        /// </summary>
        public static Guid ZHB_Otdelka_Zp = new Guid("57646ec9-0330-4822-9710-b346e7d974a9");
        /// <summary>
        ///Дата 1
        /// </summary>
        public static Guid Data_1 = new Guid("d418abc9-102a-4bef-a9a3-054520d5baca");
        /// <summary>
        ///Ток электрический
        /// </summary>
        public static Guid Tok_Elektricheskiy = new Guid("71eabcc9-fe80-4ef8-a607-d6176361a26f");
        /// <summary>
        ///Категория надежности электроснабжения по ПУЭ
        /// </summary>
        public static Guid Kategoriya_Nadezhnosti_Elektrosnabzheniya_Po_PUE = new Guid("2a70efc9-dfe4-4ae5-8f6b-30fb35bd3036");
        /// <summary>
        ///WSH_Тип стены
        /// </summary>
        public static Guid WSH_Tip_Steny = new Guid("6c3c12ca-ec8c-486b-83c1-17ef7fefef09");
        /// <summary>
        ///WSH_Тип стены_Основа
        ///Двери, окна/Экземпляр/Прочее (Параметр проекта)
        /// </summary>
        public static Guid WSH_Tip_Steny_Osnova = new Guid("8a4c12ca-5f79-4a64-84b6-28add539d05b");
        /// <summary>
        ///W_Длина балки истинная
        /// </summary>
        public static Guid W_Dlina_Balki_Istinnaya = new Guid("a29d86ca-2b12-46c2-9d84-b4fd024fbcea");
        /// <summary>
        ///W_Внешняя высота откоса на плоскости стены_(от основания до основания)
        /// </summary>
        public static Guid W_Vneshnyaya_Vysota_Otkosa_Na_Ploskosti_Steny_Ot_Osnovaniya_Do_Osnovaniya = new Guid("939cd3ca-ea55-4816-bfef-9b4a738b1e74");
        /// <summary>
        ///W_Назначение
        ///Назначение блока:&#xD&#xA- внутренний&#xD&#xA- наружный&#xD&#xA- межкомнатный&#xD&#xA- для санузлов&#xD&#xA- и др.
        /// </summary>
        public static Guid W_Naznachenie = new Guid("52da3ecb-dd3f-42bf-8cf9-87cb50867ccb");
        /// <summary>
        ///W_Внутренняя ширина откоса на плоскости стены_(основание-кирпич)
        /// </summary>
        public static Guid W_Vnutrennyaya_SHirina_Otkosa_Na_Ploskosti_Steny_Osnovaniekirpich = new Guid("3dcd59cb-a440-4f91-b31a-d5374c8a8bb3");
        /// <summary>
        ///Строка 2_Дата_Видимость
        /// </summary>
        public static Guid Stroka_2_Data_Vidimost = new Guid("5834accb-8c69-4dce-a5d6-c1355a9ef94d");
        /// <summary>
        ///W_Закладная_кол-во
        ///Тип/Строительство (Параметр семейства)
        /// </summary>
        public static Guid W_Zakladnaya_Kolvo = new Guid("a055e0cb-b5b8-41a9-9b59-ddb558e93160");
        /// <summary>
        ///W_Подоконник_Отметка от 0
        /// </summary>
        public static Guid W_Podokonnik_Otmetka_Ot_0 = new Guid("1c3c2fcc-d54a-4fb0-a75a-03ffc8065983");
        /// <summary>
        ///W_Отбойная доска_Наличие
        ///Помещения/Экземпляр/Аналитическая модель (Параметр проекта)
        /// </summary>
        public static Guid W_Otboynaya_Doska_Nalichie = new Guid("61f945cc-8d40-4e0e-a2e7-0c0a46cb25ec");
        /// <summary>
        ///W_Двери_Полотно_Ширина
        /// </summary>
        public static Guid W_Dveri_Polotno_SHirina = new Guid("0d5046cc-4260-4fc9-95ee-3abbfcb77f34");
        /// <summary>
        ///Фазное напряжение
        /// </summary>
        public static Guid Faznoe_Napryazhenie = new Guid("ddf7e3cc-aa36-45f7-b4c9-b888d414b0f8");
        /// <summary>
        ///WS_Площадь
        ///Экземпляр/Размеры
        /// </summary>
        public static Guid WS_Ploshchad = new Guid("548a28cd-dfe7-4eff-a6a1-9060bb4cc70f");
        /// <summary>
        ///W_Перемычки_лист_Обозначение
        /// </summary>
        public static Guid W_Peremychki_List_Oboznachenie = new Guid("50992dcd-e49a-4428-9243-950e21e8b558");
        /// <summary>
        ///Резерв
        /// </summary>
        public static Guid Rezerv = new Guid("043c4dcd-d1f7-4be4-bf8f-01c9d7aecf19");
        /// <summary>
        ///Комментарий
        /// </summary>
        public static Guid Kommentariy = new Guid("74da53cd-c1d1-415a-bf77-84bde4f63330");
        /// <summary>
        ///W_Двери_Усиление
        /// </summary>
        public static Guid W_Dveri_Usilenie = new Guid("a55ba0cd-1626-4f21-94a7-62681658359a");
        /// <summary>
        ///W_Доводчик
        ///Двери/Тип/Зависимости (Параметр семейства)
        /// </summary>
        public static Guid W_Dovodchik = new Guid("87520fce-1d3c-49c8-a610-4808b24d83e8");
        /// <summary>
        ///Задание ЭОМ
        /// </summary>
        public static Guid Zadanie_EOM = new Guid("3b4f54ce-02ee-4298-af2a-e69c417dd8e9");
        /// <summary>
        ///W_Альбом
        ///Экземпляр/Набор (Параметр проекта)
        /// </summary>
        public static Guid W_Albom = new Guid("52acaece-8a93-4df7-bbc0-fc8d2dbf11cc");
        /// <summary>
        ///WS_Описание отделки по К1
        ///Помещения/Экземпляр/Аналитическая модель (Параметр проекта)
        /// </summary>
        public static Guid WS_Opisanie_Otdelki_Po_K1 = new Guid("93c2d0ce-9cb6-47d1-987d-7aac15e7e2e6");
        /// <summary>
        ///Номер OmniClass
        /// </summary>
        public static Guid Nomer_OmniClass = new Guid("ed86e5ce-47e3-4c14-a288-fb3bf337e3ae");
        /// <summary>
        ///Фаза
        /// </summary>
        public static Guid Faza = new Guid("f3e3f9ce-0ab9-4054-ae44-757c76d8a838");
        /// <summary>
        ///Строка 3_Дата_Видимость
        /// </summary>
        public static Guid Stroka_3_Data_Vidimost = new Guid("396c80cf-52cc-4727-bf1c-965e152ef098");
        /// <summary>
        ///W_Пол_Сущ. отделка
        /// </summary>
        public static Guid W_Pol_Sushch_Otdelka = new Guid("7cdfe9cf-9209-4194-9a2b-9eb263ff1386");
        /// <summary>
        ///Глубина проёма
        /// </summary>
        public static Guid Glubina_Proema = new Guid("532112d0-4d6b-40ae-9936-4639838e6b9a");
        /// <summary>
        ///Коэффициент загрузки
        /// </summary>
        public static Guid Koeffitsient_Zagruzki = new Guid("17d328d0-d3c6-4b82-b0ed-cb611120c434");
        /// <summary>
        ///Примечание в спецификации
        /// </summary>
        public static Guid Primechanie_V_Spetsifikatsii = new Guid("0d1030d0-cefd-4763-8b54-08750a098f4d");
        /// <summary>
        ///W_Двери_Примечание_Класс по эксплуатационным характеристикам
        /// </summary>
        public static Guid W_Dveri_Primechanie_Klass_Po_Ekspluatatsionnym_KHarakteristikam = new Guid("6148e1d0-a6f6-4a03-b460-db24d9dea69c");
        /// <summary>
        ///W_Дефект_Зона
        /// </summary>
        public static Guid W_Defekt_Zona = new Guid("b576fed0-df42-49d9-9c45-58579260088d");
        /// <summary>
        ///W_Лестницы_Отделка низа маршей_Описание
        ///Лестницы/Экземпляр/Материалы и отделка (Параметр проекта)
        /// </summary>
        public static Guid W_Lestnitsy_Otdelka_Niza_Marshey_Opisanie = new Guid("f23a0bd1-ed5a-4045-95dd-281cb931a767");
        /// <summary>
        ///Отделка пола (описание)
        /// </summary>
        public static Guid Otdelka_Pola_Opisanie = new Guid("a311d0d1-be2a-4178-9199-9e6db807f0e7");
        /// <summary>
        ///W_Пол_Износ
        /// </summary>
        public static Guid W_Pol_Iznos = new Guid("29e8f7d1-a536-41d2-a943-46b0e26898f5");
        /// <summary>
        ///К Загрузки оборуд. 0.5..0.8
        /// </summary>
        public static Guid K_Zagruzki_Oborud_0508 = new Guid("cdcf3fd2-03de-4b96-b214-9cbf80bfd57a");
        /// <summary>
        ///Валюта
        /// </summary>
        public static Guid Valyuta = new Guid("b7d952d2-37ef-4610-a753-2ae27b979e0b");
        /// <summary>
        ///Артикул
        /// </summary>
        public static Guid Artikul_1 = new Guid("87ed74d2-498e-453b-8b28-d5a1e9542364");
        /// <summary>
        ///Высота светильников 2
        /// </summary>
        public static Guid Vysota_Svetilnikov_2 = new Guid("bb4084d2-8f39-4ad9-9ecf-a67e35507f7e");
        /// <summary>
        ///Реактивная мощность
        /// </summary>
        public static Guid Reaktivnaya_Moshchnost = new Guid("befc9fd2-9e87-42f1-986f-ab339c01648d");
        /// <summary>
        ///W_ЭлемФасада_Работы
        /// </summary>
        public static Guid W_ElemFasada_Raboty = new Guid("ba662dd3-8a5a-4ecb-8a3a-90c0f7ff315b");
        /// <summary>
        ///W_Мрк_Обозначение
        /// </summary>
        public static Guid W_Mrk_Oboznachenie = new Guid("65d435d3-a416-454a-b718-f9fcbddb91e6");
        /// <summary>
        ///Категория помещения
        /// </summary>
        public static Guid Kategoriya_Pomeshcheniya = new Guid("62d888d3-73a3-4a9c-9037-7ebd22f7fc96");
        /// <summary>
        ///Мощность_СС
        /// </summary>
        public static Guid Moshchnost_SS = new Guid("bae09bd3-d99f-4f9c-b099-dda695b8536b");
        /// <summary>
        ///W_Толщина бруса короба_(обсады)
        /// </summary>
        public static Guid W_Tolshchina_Brusa_Koroba_Obsady = new Guid("42cb2cd4-2fcc-4290-8655-e3b1cacdec44");
        /// <summary>
        ///ОВ_Вытяжка
        /// </summary>
        public static Guid OV_Vytyazhka = new Guid("bd3974d4-ba79-47c5-9b6f-af37cd7049e1");
        /// <summary>
        ///W_Потолок_Сущ. отделка
        /// </summary>
        public static Guid W_Potolok_Sushch_Otdelka = new Guid("f45cced4-7ca9-4ccb-9bff-b1f27980abe8");
        /// <summary>
        ///W_Перемычки_уголок_Обозначение
        /// </summary>
        public static Guid W_Peremychki_Ugolok_Oboznachenie = new Guid("dcae6dd5-9ed7-4e53-be5a-ba2fb97b3352");
        /// <summary>
        ///W_Высота установки
        ///Тип/Строительство (Параметр семейства)
        /// </summary>
        public static Guid W_Vysota_Ustanovki = new Guid("998d9cd5-119b-4e0d-84f0-ce693fc67b56");
        /// <summary>
        ///Длина цепи до ближайшего устройства
        /// </summary>
        public static Guid Dlina_TSepi_Do_Blizhayshego_Ustroystva = new Guid("1be3a6d5-8647-4044-be28-833b56f39086");
        /// <summary>
        ///Кондиционирование
        /// </summary>
        public static Guid Konditsionirovanie = new Guid("ad1920d6-a55a-4b5e-8d3f-8b20377e51e2");
        /// <summary>
        ///W_Проем_Длина
        /// </summary>
        public static Guid W_Proem_Dlina = new Guid("9d7e21d6-1c61-419e-b81f-4f68a33ff852");
        /// <summary>
        ///Масса изд. №3
        /// </summary>
        public static Guid Massa_Izd_3 = new Guid("2187a2d6-038c-4daa-b752-aae653022463");
        /// <summary>
        ///W_Перемычки_лист_b
        /// </summary>
        public static Guid W_Peremychki_List_b = new Guid("4342aad6-62df-4d9f-ac84-221a1d498497");
        /// <summary>
        ///W_Стены_Доп. отделка
        ///Экземпляр/Строительство (Параметр проекта)
        /// </summary>
        public static Guid W_Steny_Dop_Otdelka = new Guid("52fdefd6-ccae-4f46-836e-7b55eeacec3a");
        /// <summary>
        ///Фазность
        /// </summary>
        public static Guid Faznost = new Guid("37ed06d7-8b3f-4b1a-9dcd-a0c4ff213d79");
        /// <summary>
        ///Обозначение
        /// </summary>
        public static Guid Oboznachenie = new Guid("660641d7-fc0d-47b7-9097-571e6361730d");
        /// <summary>
        ///Дата согласования
        /// </summary>
        public static Guid Data_Soglasovaniya = new Guid("088c0ed8-d1d2-47b4-b8ab-dc2bfbf3b675");
        /// <summary>
        ///WSH_Фахверк
        ///Стены/Тип/Несущие конструкции (Параметр проекта)&#xD&#xA
        /// </summary>
        public static Guid WSH_Fakhverk = new Guid("957920d8-7796-48b6-bf6e-6ed386f269df");
        /// <summary>
        ///W_Площадь
        /// </summary>
        public static Guid W_Ploshchad = new Guid("037f4bd8-cdd1-4099-821b-3868756cca06");
        /// <summary>
        ///WH_Перемычка бетонная/газобетонная_Тип
        ///Тип/Несущие конструкции (Параметр проекта)
        /// </summary>
        public static Guid WH_Peremychka_Betonnayagazobetonnaya_Tip = new Guid("999d61d8-91b7-481e-817e-6432d9c09aee");
        /// <summary>
        ///Задание КР
        /// </summary>
        public static Guid Zadanie_KR = new Guid("e17373d8-65ff-4e5a-aa3c-e96d84f4627e");
        /// <summary>
        ///W_Закладная
        ///Тип/Строительство (Параметр семейства)
        /// </summary>
        public static Guid W_Zakladnaya = new Guid("226776d8-1240-4ec3-a97f-bf0b28cb11b2");
        /// <summary>
        ///W_Двери_Полотно_Высота
        /// </summary>
        public static Guid W_Dveri_Polotno_Vysota = new Guid("8da6a3d8-710f-4f8b-8b6d-5c1ab9d35f16");
        /// <summary>
        ///Дата 6
        /// </summary>
        public static Guid Data_6 = new Guid("b10090d9-61c1-427c-84c0-60fbbef81cc9");
        /// <summary>
        ///ОВ_Система В
        /// </summary>
        public static Guid OV_Sistema_V = new Guid("ac3d92d9-87a3-438c-81b9-ad47e4f07564");
        /// <summary>
        ///Длина цепи до наиболее удаленного устройства
        /// </summary>
        public static Guid Dlina_TSepi_Do_Naibolee_Udalennogo_Ustroystva = new Guid("541bbad9-d80d-48b1-b0b5-1460a13a8d66");
        /// <summary>
        ///W_Тип порога
        ///Двери/Тип/Зависимости (Параметр семейства)
        /// </summary>
        public static Guid W_Tip_Poroga = new Guid("da5ec2d9-73bd-4ce2-b36e-5fcb2074c3ce");
        /// <summary>
        ///Коэффициент теплопроводности
        /// </summary>
        public static Guid Koeffitsient_Teploprovodnosti = new Guid("e1b1c5d9-d04e-4535-a581-d6ed0e527e20");
        /// <summary>
        ///4_Лист
        /// </summary>
        public static Guid List4_ = new Guid("6f671ada-deee-4c02-9908-ec6d44edc4e2");
        /// <summary>
        ///W_Реставрационная зона
        /// </summary>
        public static Guid W_Restavratsionnaya_Zona = new Guid("b7c22ada-e9d6-4536-95cb-de273916eda3");
        /// <summary>
        ///АР-Номер
        /// </summary>
        public static Guid ARNomer = new Guid("5aa935da-7174-49d6-99da-6c02d2145984");
        /// <summary>
        ///Высота светильников
        /// </summary>
        public static Guid Vysota_Svetilnikov = new Guid("d54096da-fedd-4124-b9ad-cfd730586ea4");
        /// <summary>
        ///W_Рентгензащита
        ///Двери/Тип/Зависимости (Параметр семейства)
        /// </summary>
        public static Guid W_Rentgenzashchita = new Guid("b6cebada-dc11-49fd-8aab-0c71266b6cad");
        /// <summary>
        ///ОВ_Система П
        /// </summary>
        public static Guid OV_Sistema_P = new Guid("54cbdeda-e9d5-42f5-9ca0-ee104081f228");
        /// <summary>
        ///WH_Основа черновой отделки
        ///Тип/Аналитическая модель (Праметры проекта)
        /// </summary>
        public static Guid WH_Osnova_CHernovoy_Otdelki = new Guid("225011db-5f5d-47a3-90cf-b554763660cf");
        /// <summary>
        ///W_Ширина
        /// </summary>
        public static Guid W_SHirina = new Guid("0a8476db-7bed-4e2e-9c1c-7521d9ea6fcc");
        /// <summary>
        ///Позиция
        /// </summary>
        public static Guid Pozitsiya = new Guid("87a47ddb-c46c-4d59-9235-b188f5cadee4");
        /// <summary>
        ///Пол_Номера помещений
        /// </summary>
        public static Guid Pol_Nomera_Pomeshcheniy = new Guid("848fcfdb-e8e6-4c1a-b3e3-4c0e79641536");
        /// <summary>
        ///Листов в альбоме
        /// </summary>
        public static Guid Listov_V_Albome = new Guid("e0ec1edc-62ad-4f8b-bc73-054144e0e09e");
        /// <summary>
        ///Отделка потолка (описание)
        /// </summary>
        public static Guid Otdelka_Potolka_Opisanie = new Guid("4930d7dc-06cc-4e5b-bd4e-7f849593f984");
        /// <summary>
        ///WH_Условия эксплуатации
        ///Помещения/Экземпляр/Текст (Параметр проекта)
        /// </summary>
        public static Guid WH_Usloviya_Ekspluatatsii = new Guid("759ffadc-e3e3-45a4-a3e3-2341916cff7e");
        /// <summary>
        ///Полная мощность
        /// </summary>
        public static Guid Polnaya_Moshchnost = new Guid("6d520add-b176-44a2-8901-bb70d4be8f13");
        /// <summary>
        ///Наименование товара
        /// </summary>
        public static Guid Naimenovanie_Tovara = new Guid("7bbd0bdd-d876-450b-a133-74180d1561c1");
        /// <summary>
        ///W_Глубина проёма под короб_(с учётом зазоров)
        /// </summary>
        public static Guid W_Glubina_Proema_Pod_Korob_S_Uchetom_Zazorov = new Guid("a1022ddd-5a9d-4b16-96f1-f0046b49da00");
        /// <summary>
        ///У.в.
        /// </summary>
        public static Guid Uv = new Guid("886d0ede-1863-4d58-82e9-33e4c2aeda1a");
        /// <summary>
        ///Количество людей
        /// </summary>
        public static Guid Kolichestvo_Lyudey = new Guid("697056de-88d6-4b1c-af33-b1a87a498ca9");
        /// <summary>
        ///Предварительный номинальный рабочий ток вводного аппарата
        /// </summary>
        public static Guid Predvaritelnyy_Nominalnyy_Rabochiy_Tok_Vvodnogo_Apparata = new Guid("1f4162de-6422-41ca-bbda-0152a06407fe");
        /// <summary>
        ///Максимальный уровень звукового давления
        /// </summary>
        public static Guid Maksimalnyy_Uroven_Zvukovogo_Davleniya = new Guid("a15762de-857b-4fec-a7a7-253eee8f62d8");
        /// <summary>
        ///Напряжение_СС
        /// </summary>
        public static Guid Napryazhenie_SS = new Guid("a1836dde-6b7c-420b-bff1-ce9f023f05c8");
        /// <summary>
        ///W_Тип элемента КМ
        /// </summary>
        public static Guid W_Tip_Elementa_KM = new Guid("034086de-9c81-4c09-9231-f4f3024dab97");
        /// <summary>
        ///WH_Стены_Тип отделки
        ///Экземпляр/Строительство (Параметр проекта)
        /// </summary>
        public static Guid WH_Steny_Tip_Otdelki = new Guid("7db6a7de-1f9e-4432-a7e3-625ca3115b1e");
        /// <summary>
        ///Отделение
        /// </summary>
        public static Guid Otdelenie = new Guid("5e1729df-c131-4dcb-90f8-f5d95c2714ac");
        /// <summary>
        ///К1 отделка з.п.
        /// </summary>
        public static Guid K1_Otdelka_Zp = new Guid("d7a56fdf-659f-49f1-94d8-967d49614556");
        /// <summary>
        ///cos F
        /// </summary>
        public static Guid cos_F = new Guid("2ca28edf-3aaf-486a-830a-fae82079832d");
        /// <summary>
        ///Потолок_Тип
        /// </summary>
        public static Guid Potolok_Tip = new Guid("61af27e0-19a7-49b9-9ef3-1216098dd2cf");
        /// <summary>
        ///Запас длины трубы для электрической цепи
        /// </summary>
        public static Guid Zapas_Dliny_Truby_Dlya_Elektricheskoy_TSepi = new Guid("25122ee0-d761-4a5f-af49-b507b64188e3");
        /// <summary>
        ///Уровень шума, дБ
        /// </summary>
        public static Guid Uroven_SHuma_DB = new Guid("02e23fe0-ec72-43f3-a0f3-0e0c98faf07d");
        /// <summary>
        ///Наличие газов в помещении
        /// </summary>
        public static Guid Nalichie_Gazov_V_Pomeshchenii = new Guid("63e952e0-bd4a-4b75-871c-8a4523784041");
        /// <summary>
        ///Стоимость
        /// </summary>
        public static Guid Stoimost = new Guid("83cac6e0-4b3f-47d6-baf8-da02b1f8bd5e");
        /// <summary>
        ///Количество светильников 2
        /// </summary>
        public static Guid Kolichestvo_Svetilnikov_2 = new Guid("67addee0-a4dd-4cba-bfb2-5e74a713a187");
        /// <summary>
        ///d - Диаметр
        /// </summary>
        public static Guid d__Diametr = new Guid("c71e01e1-afe8-42ac-a37a-4a59813ed436");
        /// <summary>
        ///Количество для спецификации
        /// </summary>
        public static Guid Kolichestvo_Dlya_Spetsifikatsii = new Guid("0c9340e1-f897-4185-9f93-d3067e165b78");
        /// <summary>
        ///W_КМ_Значок профиля
        /// </summary>
        public static Guid W_KM_Znachok_Profilya = new Guid("19f646e1-4117-4048-aaaa-af43953e3fd8");
        /// <summary>
        ///Номинальный ток для выбора АВ
        /// </summary>
        public static Guid Nominalnyy_Tok_Dlya_Vybora_AV = new Guid("d15d6de1-37e4-4102-8fb9-7d1722f90413");
        /// <summary>
        ///W_Стадия
        ///Экземпляр/Набор (Параметр проекта)
        /// </summary>
        public static Guid W_Stadiya = new Guid("907a74e1-c3f9-46b7-8143-c8f11cce922e");
        /// <summary>
        ///Другое
        /// </summary>
        public static Guid Drugoe = new Guid("fac683e1-5024-4a1c-92e6-7826915d13c8");
        /// <summary>
        ///W_Обозначение
        ///Двери/Тип/Зависимости (Параметр семейства)
        /// </summary>
        public static Guid W_Oboznachenie = new Guid("b3f6a9e1-0a9e-430f-9313-bf3bf41808d4");
        /// <summary>
        ///Примечания
        /// </summary>
        public static Guid Primechaniya = new Guid("5013cfe1-46e8-410f-9261-e4b53151c6e9");
        /// <summary>
        ///WS_Стены_Площадь К1
        ///Экземпляр/Аналитическая модель (Параметр проекта)
        /// </summary>
        public static Guid WS_Steny_Ploshchad_K1 = new Guid("40f9f1e1-9d78-4102-8e0f-bb4a11e412dc");
        /// <summary>
        ///Изделие №4
        /// </summary>
        public static Guid Izdelie_4 = new Guid("c8c82be2-7205-4f55-a17c-27255dc546e7");
        /// <summary>
        ///Строка 5 должность
        /// </summary>
        public static Guid Stroka_5_Dolzhnost = new Guid("7e974de2-5e90-4af8-ba6a-601fe0c76b0f");
        /// <summary>
        ///Марка
        /// </summary>
        public static Guid Marka = new Guid("c28e72e2-a0b6-4049-aab0-9a59f5ad381e");
        /// <summary>
        ///W_Двери_Примечание_Тип отделки
        /// </summary>
        public static Guid W_Dveri_Primechanie_Tip_Otdelki = new Guid("d4aacfe2-499e-4b3f-9085-7aad6c319413");
        /// <summary>
        ///Напряжение
        /// </summary>
        public static Guid Napryazhenie = new Guid("8d80d9e2-62e2-4be3-897c-44c45a21d9f2");
        /// <summary>
        ///W_Двери_Примечание_Охранная сигнализация
        /// </summary>
        public static Guid W_Dveri_Primechanie_Okhrannaya_Signalizatsiya = new Guid("5de0f4e2-fb50-48f1-8fc3-ec1ddb487959");
        /// <summary>
        ///Этаж
        ///Помещения/Экземпляр/Идентификация (Параметр проекта)
        /// </summary>
        public static Guid Etazh = new Guid("cbb309e3-d4bf-4ea5-ae24-c58c6194cb61");
        /// <summary>
        ///W_Лестницы_Отделка плинтуса_Длина
        ///Лестницы/Экземпляр/Материалы и отделка (Параметр проекта)
        /// </summary>
        public static Guid W_Lestnitsy_Otdelka_Plintusa_Dlina = new Guid("12fd47e3-c8a6-4a07-8891-061e2f7f3452");
        /// <summary>
        ///Коэффициент спроса в щитах
        /// </summary>
        public static Guid Koeffitsient_Sprosa_V_SHCHitakh = new Guid("85287be3-fe67-4968-ace6-59f0b861cb4b");
        /// <summary>
        ///Способ прокладки в щитах
        /// </summary>
        public static Guid Sposob_Prokladki_V_SHCHitakh = new Guid("3744dde3-f247-47ed-b300-91078c2942d2");
        /// <summary>
        ///WS_Пол_Номера помещений по типу
        ///Экземпляр/Аналитическая модель (Параметр проекта)
        /// </summary>
        public static Guid WS_Pol_Nomera_Pomeshcheniy_Po_Tipu = new Guid("1eb3e9e3-d0f1-491e-ad81-8b289f626282");
        /// <summary>
        ///WS_Описание отделки по ГК
        ///Помещения/Экземпляр/Аналитическая модель (Параметр проекта)
        /// </summary>
        public static Guid WS_Opisanie_Otdelki_Po_GK = new Guid("13f500e4-e661-4f22-b51c-d11e7ff0b82f");
        /// <summary>
        ///W_Откосы_Вн_У стены_Ширина
        /// </summary>
        public static Guid W_Otkosy_Vn_U_Steny_SHirina = new Guid("f76521e4-612e-48ed-a2a2-b8401ceb6a2b");
        /// <summary>
        ///W_Пол_Толщина
        ///Помещения/Экземпляр/Строительство (Параметр проекта)
        /// </summary>
        public static Guid W_Pol_Tolshchina = new Guid("07667be4-6a1c-42f5-abbe-d9a98b53afb3");
        /// <summary>
        ///Принадлежность к оборудованию
        /// </summary>
        public static Guid Prinadlezhnost_K_Oborudovaniyu = new Guid("dacabde4-565e-4c65-b952-cbb319504534");
        /// <summary>
        ///W_Потолок_Конструкция
        ///Экземпляр/Строительство (Параметр проекта)
        /// </summary>
        public static Guid W_Potolok_Konstruktsiya = new Guid("0e7ef5e4-24c1-4a58-90b5-6ca49967c4b0");
        /// <summary>
        ///Сайт разработчика
        /// </summary>
        public static Guid Sayt_Razrabotchika = new Guid("627bf7e4-4aac-42d4-804f-dcc4c1efed7f");
        /// <summary>
        ///Дата 5
        /// </summary>
        public static Guid Data_5 = new Guid("d8a5b2e5-369b-424d-9537-95f050d153be");
        /// <summary>
        ///Контрольные цепи
        /// </summary>
        public static Guid Kontrolnye_TSepi = new Guid("0f13e1e5-71bb-4b0f-b3dc-18054c25e1ee");
        /// <summary>
        ///Изменение 7
        /// </summary>
        public static Guid Izmenenie_7 = new Guid("29e3f0e5-a302-48dd-8d5f-de21ce256bf8");
        /// <summary>
        ///Толщина стены
        /// </summary>
        public static Guid Tolshchina_Steny = new Guid("18d501e6-fea0-4ada-abb6-74cb64e3e9b2");
        /// <summary>
        ///ТВ
        /// </summary>
        public static Guid TV_1 = new Guid("fc1815e6-98c8-4a40-9260-59fefaea5460");
        /// <summary>
        ///Ось Н
        /// </summary>
        public static Guid Os_N = new Guid("67bb47e6-5778-4cdd-be18-f22af0b684cc");
        /// <summary>
        ///W_Перемычки_толщина
        /// </summary>
        public static Guid W_Peremychki_Tolshchina = new Guid("fde361e6-51d5-4528-9c7a-268264c82529");
        /// <summary>
        ///W_Откосы_Нар_У заполнения_Высота
        /// </summary>
        public static Guid W_Otkosy_Nar_U_Zapolneniya_Vysota = new Guid("5a9ba9e6-5033-4581-b1ca-87796b5659b8");
        /// <summary>
        ///Коэффициент ассимиляции тепла
        /// </summary>
        public static Guid Koeffitsient_Assimilyatsii_Tepla = new Guid("1eefc9e6-c00e-4125-b9f0-0b5102fbcdb8");
        /// <summary>
        ///W_Кол-во открываний
        /// </summary>
        public static Guid W_Kolvo_Otkryvaniy = new Guid("faa2ece6-0ff6-460b-806c-4d10f7823369");
        /// <summary>
        ///02_Открывание окна
        ///Открывание окна - Правая/Левая створка&#xD&#xAЗначения: П,Л
        /// </summary>
        public static Guid Otkryvanie_Okna02_ = new Guid("fceaeee6-c103-4a56-82c9-2ddb71de925b");
        /// <summary>
        ///Помещение
        /// </summary>
        public static Guid Pomeshchenie = new Guid("ed6f5ce7-08c7-4668-8c0d-2aed8434766c");
        /// <summary>
        ///W_Перемычки_количество_уголков
        /// </summary>
        public static Guid W_Peremychki_Kolichestvo_Ugolkov = new Guid("7d3865e7-0add-4018-b9f4-9e270a00f2e5");
        /// <summary>
        ///Уровень шума
        /// </summary>
        public static Guid Uroven_SHuma = new Guid("c5a6cee7-413f-4242-b044-f89ed3d12c9f");
        /// <summary>
        ///Длина простенка
        /// </summary>
        public static Guid Dlina_Prostenka = new Guid("92aa17e8-2179-4384-9a86-61ead2a45864");
        /// <summary>
        ///Полный объем
        /// </summary>
        public static Guid Polnyy_Obem = new Guid("c3ea1be8-157a-48dc-afb8-ff7281a31d15");
        /// <summary>
        ///Дизайн проект
        /// </summary>
        public static Guid Dizayn_Proekt = new Guid("882023e8-ac6d-4b28-a6a2-da85f609d9b3");
        /// <summary>
        ///W_Черновая отделка з.п.
        ///Экземпляр/Результаты анализа
        /// </summary>
        public static Guid W_CHernovaya_Otdelka_Zp = new Guid("9965dee8-1c32-4b08-be26-e679b10647dd");
        /// <summary>
        ///W_Высота стены (фахверк)
        /// </summary>
        public static Guid W_Vysota_Steny_Fakhverk = new Guid("dd6203e9-30f5-4d9a-8bf3-62c98d759d95");
        /// <summary>
        ///W_Внутренняя высота откоса на плоскости стены_(от основания до основания)
        /// </summary>
        public static Guid W_Vnutrennyaya_Vysota_Otkosa_Na_Ploskosti_Steny_Ot_Osnovaniya_Do_Osnovaniya = new Guid("cb045ae9-dfc6-4962-af5b-17877f5a4fd7");
        /// <summary>
        ///_Номер
        /// </summary>
        public static Guid Nomer_ = new Guid("4b0593e9-781e-4b45-8aaa-2810595e5e1a");
        /// <summary>
        ///WH_Усиление дв.проема_Тип
        ///Тип/Несущие конструкции (Параметр проекта)
        /// </summary>
        public static Guid WH_Usilenie_Dvproema_Tip = new Guid("9e8ecbe9-7484-4214-ab48-f0efa0e008fa");
        /// <summary>
        ///h - Высота
        /// </summary>
        public static Guid h__Vysota = new Guid("f79acee9-d657-4927-9ef9-3974443fb27a");
        /// <summary>
        ///WS_Наличие галтелей
        /// </summary>
        public static Guid WS_Nalichie_Galteley = new Guid("d1708eea-58e4-4175-8f61-e6c16354695f");
        /// <summary>
        ///Режим работы
        /// </summary>
        public static Guid Rezhim_Raboty_1 = new Guid("0ef7a7ea-a7b3-4798-910a-616704576c4f");
        /// <summary>
        ///W_Двери_Монтажный_проём_ширина
        /// </summary>
        public static Guid W_Dveri_Montazhnyy_Proem_SHirina = new Guid("e25fabea-9ca3-4ec8-a23c-5f78768afda2");
        /// <summary>
        ///W_Масса погонного метра
        /// </summary>
        public static Guid W_Massa_Pogonnogo_Metra = new Guid("ca2ef6ea-54da-4380-bdd2-9c780ec4cdf0");
        /// <summary>
        ///W_Двери_Примечание_Датчик_движения
        /// </summary>
        public static Guid W_Dveri_Primechanie_Datchik_Dvizheniya = new Guid("1ddb7deb-5f54-4ba7-8f52-dee635a16008");
        /// <summary>
        ///К.
        /// </summary>
        public static Guid K = new Guid("9ce2f3eb-c051-46da-b6c5-36373913df37");
        /// <summary>
        ///Класс чистоты
        /// </summary>
        public static Guid Klass_CHistoty = new Guid("fda109ec-d8a7-4bc4-a94d-707027b65985");
        /// <summary>
        ///_Замечание
        /// </summary>
        public static Guid Zamechanie_ = new Guid("2a3e1cec-0b56-4e59-87cc-da10e103cb88");
        /// <summary>
        ///Строка 6_Дата_Видимость
        /// </summary>
        public static Guid Stroka_6_Data_Vidimost = new Guid("180b29ec-c263-43ea-a69a-68854b6315ba");
        /// <summary>
        ///W_Проем_Пробиваемый
        /// </summary>
        public static Guid W_Proem_Probivaemyy = new Guid("831b60ec-4b81-4413-a150-a528692416bd");
        /// <summary>
        ///WS_ВОР_Единица измерения_Масса
        ///Все категории/Экземпляр/Прочее (Параметр проекта)
        /// </summary>
        public static Guid WS_VOR_Edinitsa_Izmereniya_Massa = new Guid("200475ec-eb34-4028-97ed-3f2662571c8d");
        /// <summary>
        ///W_Данные элементов пола
        /// </summary>
        public static Guid W_Dannye_Elementov_Pola = new Guid("bc2a8dec-f25a-478e-b126-cec519ab8981");
        /// <summary>
        ///5_Лист
        /// </summary>
        public static Guid List5_ = new Guid("51a9c0ec-1a4a-4a5a-9cac-3f6ba2688778");
        /// <summary>
        ///Минимальная площадь
        /// </summary>
        public static Guid Minimalnaya_Ploshchad = new Guid("0a20cbec-adf4-4c14-9d3e-bfe651008f05");
        /// <summary>
        ///W_Фильтр АР
        /// </summary>
        public static Guid W_Filtr_AR = new Guid("0ebc18ed-111f-4089-b8d8-ccd7c3133557");
        /// <summary>
        ///Классы отходов
        /// </summary>
        public static Guid Klassy_Otkhodov = new Guid("920e22ed-8109-4858-a794-e8650a1424b8");
        /// <summary>
        ///Высота
        /// </summary>
        public static Guid Vysota_1 = new Guid("311a3fed-edbc-4eaa-96e0-ba9cab1a6c46");
        /// <summary>
        ///Группа вида
        /// </summary>
        public static Guid Gruppa_Vida = new Guid("4ef474ed-2b99-4fba-a1cb-6c02cce07cec");
        /// <summary>
        ///W_Плинтус_Описание
        ///Помещения/Экземпляр/Аналитическая модель (Параметр проекта)
        /// </summary>
        public static Guid W_Plintus_Opisanie = new Guid("5af887ed-90b6-465e-895b-9e41d46b2bfe");
        /// <summary>
        ///Дисциплина проёма
        /// </summary>
        public static Guid Distsiplina_Proema = new Guid("dee48ded-481a-47a0-a623-491fb42faf44");
        /// <summary>
        ///Крышка
        /// </summary>
        public static Guid Kryshka = new Guid("86749bed-4491-45ad-8256-1487418ec7d7");
        /// <summary>
        ///Длина линии в щитах
        /// </summary>
        public static Guid Dlina_Linii_V_SHCHitakh = new Guid("2a12c5ed-7b33-4360-9603-7dca9503e670");
        /// <summary>
        ///Уникальный идентификатор
        /// </summary>
        public static Guid Unikalnyy_Identifikator = new Guid("d789b9ee-55bc-4c61-8099-7d50bde41b75");
        /// <summary>
        ///Задание АР
        /// </summary>
        public static Guid Zadanie_AR = new Guid("a8f238ef-f3f3-412d-9e0d-d24b9dc6e16b");
        /// <summary>
        ///Упаковка
        /// </summary>
        public static Guid Upakovka = new Guid("23911ff0-29fc-4b91-a67d-1608ee0e416d");
        /// <summary>
        ///Уровни клининга
        /// </summary>
        public static Guid Urovni_Klininga = new Guid("6e5d40f0-8565-4a5b-b807-9646ba8b112e");
        /// <summary>
        ///WS_Описание отделки з.п.
        ///Помещения/Экземпляр/Аналитическая модель (Параметр проекта)
        /// </summary>
        public static Guid WS_Opisanie_Otdelki_Zp = new Guid("99ed69f0-b545-4dce-a607-96b8c7114450");
        /// <summary>
        ///W_Лестницы_Отделка ступеней и площадок_Тип
        ///Лестницы/Экземпляр/Материалы и отделка (Параметр проекта)
        /// </summary>
        public static Guid W_Lestnitsy_Otdelka_Stupeney_I_Ploshchadok_Tip = new Guid("3bb185f0-f1e9-43ec-b8f1-918eadabbbff");
        /// <summary>
        ///W_Нижний монтажный зазор
        /// </summary>
        public static Guid W_Nizhniy_Montazhnyy_Zazor = new Guid("03baddf0-e75d-42e9-b99e-63eb7703578b");
        /// <summary>
        ///Аварийное освещение
        /// </summary>
        public static Guid Avariynoe_Osveshchenie = new Guid("8eaedef0-046f-45cb-916d-fa3ae5e8f6e1");
        /// <summary>
        ///Ось Н помещения
        /// </summary>
        public static Guid Os_N_Pomeshcheniya = new Guid("cb914cf1-f6ee-4660-ac3e-eb017f4fbffa");
        /// <summary>
        ///Иное
        /// </summary>
        public static Guid Inoe = new Guid("6b44d8f1-bc5e-482b-b965-82d741816f3c");
        /// <summary>
        ///Категория пожарной опасности
        /// </summary>
        public static Guid Kategoriya_Pozharnoy_Opasnosti = new Guid("a1a7f9f1-2dfe-4b52-9ac6-c0649adc5eef");
        /// <summary>
        ///Сопротивление трансформатора (Zтр/3)
        /// </summary>
        public static Guid Soprotivlenie_Transformatora_Ztr3 = new Guid("98cd34f2-613c-43c6-b592-1a51fc1a61dc");
        /// <summary>
        ///W_Двери_Примечание_импост
        /// </summary>
        public static Guid W_Dveri_Primechanie_Impost = new Guid("2e1c3df2-9707-41ca-84d1-4bb0e8a17344");
        /// <summary>
        ///Многострочный параметр_тест
        /// </summary>
        public static Guid Mnogostrochnyy_Parametr_Test = new Guid("f91a6af2-ef73-4b5f-bea4-a11ba634f80b");
        /// <summary>
        ///W_Заполнение_Ширина
        /// </summary>
        public static Guid W_Zapolnenie_SHirina = new Guid("071077f2-b8e1-4979-b793-3a242e5c8171");
        /// <summary>
        ///Косинус в щитах
        /// </summary>
        public static Guid Kosinus_V_SHCHitakh = new Guid("ddc9d5f2-202b-4f16-a5d8-f1180c8b7984");
        /// <summary>
        ///WH_Перемычка металлическая_Тип
        ///Тип/Несущие конструкции (Параметр проекта)
        /// </summary>
        public static Guid WH_Peremychka_Metallicheskaya_Tip = new Guid("33120ff3-3570-4284-a088-8066803c9565");
        /// <summary>
        ///Крепление
        /// </summary>
        public static Guid Kreplenie = new Guid("5daa2ff3-e1c8-4cf4-ae2c-c5074b20688c");
        /// <summary>
        ///W_Категория помещения
        ///Помещения/Экземпляр/Текст (Параметр проекта)
        /// </summary>
        public static Guid W_Kategoriya_Pomeshcheniya = new Guid("d24742f3-7bc0-440c-a2c8-51e9576eabb8");
        /// <summary>
        ///Категория по ПУЭ
        /// </summary>
        public static Guid Kategoriya_Po_PUE = new Guid("0a68adf3-3bcf-4cf8-9d9f-9cf24aeac159");
        /// <summary>
        ///W_Перемычки_длина
        /// </summary>
        public static Guid W_Peremychki_Dlina = new Guid("e337cff3-4b8c-418a-a0a6-7a08d8567ba5");
        /// <summary>
        ///W_Марка
        ///Двери/Тип/Зависимости (Параметр семейства)
        /// </summary>
        public static Guid W_Marka = new Guid("bf83d3f3-592d-4569-8244-59f5a46582eb");
        /// <summary>
        ///W_Перемычка_Поз_лист
        /// </summary>
        public static Guid W_Peremychka_Poz_List = new Guid("410f0cf4-2407-4758-9ab0-902f98bc7913");
        /// <summary>
        ///W_Толщина стены
        /// </summary>
        public static Guid W_Tolshchina_Steny = new Guid("c9bf40f4-a859-417b-be85-a28568439c6b");
        /// <summary>
        ///Категория в спецификации
        /// </summary>
        public static Guid Kategoriya_V_Spetsifikatsii = new Guid("7d0367f4-fcc3-41a1-9162-81ab4bd243db");
        /// <summary>
        ///1_Дата
        /// </summary>
        public static Guid Data1_ = new Guid("264f74f4-49f6-4f7b-ad3e-079baaedd897");
        /// <summary>
        ///Ось разделения
        /// </summary>
        public static Guid Os_Razdeleniya = new Guid("204d7bf4-0a20-473a-a457-07873b386fcf");
        /// <summary>
        ///W_Откосы_Ширина
        ///Окна/Экземпляр/Материалы и отделка (Параметр семейства)
        /// </summary>
        public static Guid W_Otkosy_SHirina = new Guid("ea1b8df4-ab20-4cc5-8f2a-8aea549aa8ae");
        /// <summary>
        ///_Местный отсос
        /// </summary>
        public static Guid Mestnyy_Otsos_ = new Guid("5c11caf4-aaa9-4edc-80ec-dfa4c04d7def");
        /// <summary>
        ///IDroom
        /// </summary>
        public static Guid IDroom = new Guid("542026f5-b074-49d2-a8ba-072cbd593b6a");
        /// <summary>
        ///К Факт. выдел. тепла 0.1..1
        /// </summary>
        public static Guid K_Fakt_Vydel_Tepla_011 = new Guid("8db202f6-c8de-4b49-8fc2-073b0fba3d42");
        /// <summary>
        ///W_Шелыга_Высота
        /// </summary>
        public static Guid W_SHelyga_Vysota = new Guid("34931df6-b626-4cc6-9a97-2b8af40a2cb3");
        /// <summary>
        ///ID электрического щита
        /// </summary>
        public static Guid ID_Elektricheskogo_SHCHita = new Guid("05bc61f6-87f3-43d3-b70f-a6e72a2b7807");
        /// <summary>
        ///Ширина полосы горизонтального заземлителя
        /// </summary>
        public static Guid SHirina_Polosy_Gorizontalnogo_Zazemlitelya = new Guid("5dfa92f6-5446-4a9a-919b-3a1d165e766d");
        /// <summary>
        ///W_Номер(вручную)
        ///Экземпляр/? (Параметр проекта)
        /// </summary>
        public static Guid W_Nomervruchnuyu = new Guid("0145adf6-46dc-4b37-b6bb-dfa5ba1c79e7");
        /// <summary>
        ///Условия_окружающей_среды
        /// </summary>
        public static Guid Usloviya_Okruzhayushchey_Sredy = new Guid("0cacfef6-bedb-48b6-aa3b-0789701ace22");
        /// <summary>
        ///Глубина
        /// </summary>
        public static Guid Glubina = new Guid("c87d07f7-f540-4380-ae80-c600a1d3d76e");
        /// <summary>
        ///W_Двери_Обозначение_Фрамуга
        ///Наличие фрамуги в двери&#xD&#xA
        /// </summary>
        public static Guid W_Dveri_Oboznachenie_Framuga = new Guid("906b35f7-5989-4915-9de1-9e4843e6c1f7");
        /// <summary>
        ///Сопротивление теплопередаче пола
        /// </summary>
        public static Guid Soprotivlenie_Teploperedache_Pola = new Guid("0fa059f7-a79e-458f-8aa9-0202fa3c6986");
        /// <summary>
        ///W_Двери_Примечания_Рентгенозащита
        /// </summary>
        public static Guid W_Dveri_Primechaniya_Rentgenozashchita = new Guid("b6b869f7-6a41-4478-871f-4b4e51089b58");
        /// <summary>
        ///Общее активное сопротивление, мОм
        /// </summary>
        public static Guid Obshchee_Aktivnoe_Soprotivlenie_MOm = new Guid("37e879f7-5971-40a8-8f77-7752d59771ad");
        /// <summary>
        ///_Система Вытяжки
        /// </summary>
        public static Guid Sistema_Vytyazhki_ = new Guid("fd7682f7-b637-4cea-9e05-5ff32ff41ad4");
        /// <summary>
        ///WS_ВОР_Конструкция потолка
        ///Помещения/Экземпляр/Прочее (Параметр проекта)
        /// </summary>
        public static Guid WS_VOR_Konstruktsiya_Potolka = new Guid("bb8489f7-a72d-4146-9d5b-725019dd0531");
        /// <summary>
        ///2_Номер изменения
        /// </summary>
        public static Guid Nomer_Izmeneniya2_ = new Guid("4a93abf7-4c19-4379-aeec-27930b3fde1a");
        /// <summary>
        ///W_Стены_Сущ. отделка
        /// </summary>
        public static Guid W_Steny_Sushch_Otdelka = new Guid("1d51adf7-a401-40d7-a51b-d0e90b4c904f");
        /// <summary>
        ///Цена
        /// </summary>
        public static Guid TSena = new Guid("6c03f3f7-1d4f-4c35-a1d8-61e2aecd647e");
        /// <summary>
        ///_Температура притока заданная
        /// </summary>
        public static Guid Temperatura_Pritoka_Zadannaya_ = new Guid("be4f60f8-7fc4-4e22-a410-ab621f5cd1f5");
        /// <summary>
        ///Толщина отделки стен
        /// </summary>
        public static Guid Tolshchina_Otdelki_Sten = new Guid("4b7bb7f8-99cf-4fef-9525-77c628b57e44");
        /// <summary>
        ///О2
        /// </summary>
        public static Guid O2 = new Guid("8825c7f8-7975-44f2-9077-7fd558b8f68e");
        /// <summary>
        ///Группа оборудования
        /// </summary>
        public static Guid Gruppa_Oborudovaniya = new Guid("7b55e8f8-1c3c-4f42-807a-ca52b10813db");
        /// <summary>
        ///Температура наружного воздуха (холодный период)
        /// </summary>
        public static Guid Temperatura_Naruzhnogo_Vozdukha_KHolodnyy_Period = new Guid("414e01f9-f6e8-44bf-8114-08e65319479a");
        /// <summary>
        ///W_Шахта_Высота от отм. верхней остановки
        ///Оборудование/Тип/Размеры (Параметр семейства)
        /// </summary>
        public static Guid W_SHakhta_Vysota_Ot_Otm_Verkhney_Ostanovki = new Guid("53bf1ff9-fae6-4c31-94f9-98a3051f52ac");
        /// <summary>
        ///Д.в.
        /// </summary>
        public static Guid Dv = new Guid("408d31f9-7437-446c-8025-89e4839ba995");
        /// <summary>
        ///Тип оборудования
        /// </summary>
        public static Guid Tip_Oborudovaniya = new Guid("d2af3cf9-023e-49e7-839b-44bbb34c29a0");
        /// <summary>
        ///W_Фахверк_Высота
        ///Обощенные модели/Тип/Размеры (Параметр семейства)
        /// </summary>
        public static Guid W_Fakhverk_Vysota = new Guid("04513ff9-cbb5-4ff6-b597-4f98ea332843");
        /// <summary>
        ///W_Подоконник_Описание
        ///Окна/Тип/Материалы и отделка (Параметр семейства)
        /// </summary>
        public static Guid W_Podokonnik_Opisanie = new Guid("26aa7df9-f57d-42fa-9849-15420208a53f");
        /// <summary>
        ///Подключение к воде
        /// </summary>
        public static Guid Podklyuchenie_K_Vode = new Guid("bf03fff9-8d69-4579-872f-df875149a1b3");
        /// <summary>
        ///W_Книга
        ///Экземпляр/Набор (Параметр проекта)
        /// </summary>
        public static Guid W_Kniga = new Guid("56ab19fa-5209-4cb5-a9d5-b64fa4e6bf7f");
        /// <summary>
        ///W_Шахта_Глубина
        ///Оборудование/Тип/Размеры (Параметр семейства)
        /// </summary>
        public static Guid W_SHakhta_Glubina = new Guid("ebb337fa-6ef2-408a-8eb0-92d0e435a6bc");
        /// <summary>
        ///Радиорозетка
        /// </summary>
        public static Guid Radiorozetka = new Guid("f090aafa-ed3e-414d-8c8c-a79533c95119");
        /// <summary>
        ///Количество ламп
        /// </summary>
        public static Guid Kolichestvo_Lamp = new Guid("25defafa-38b6-4d26-9b33-8605dae1d50c");
        /// <summary>
        ///Связь
        /// </summary>
        public static Guid Svyaz = new Guid("e8aa04fb-98c7-46c8-ade3-f45b380e0840");
        /// <summary>
        ///Изменение
        /// </summary>
        public static Guid Izmenenie = new Guid("06f32bfb-1b08-41eb-b423-100a00794c78");
        /// <summary>
        ///Масса изд. №5
        /// </summary>
        public static Guid Massa_Izd_5 = new Guid("767946fb-fe0f-47c3-af2c-0e32ecd775a8");
        /// <summary>
        ///W_Откосы_Нар_У стены_Высота
        /// </summary>
        public static Guid W_Otkosy_Nar_U_Steny_Vysota = new Guid("152192fb-29e6-4da4-8547-4df2c5dfe8c4");
        /// <summary>
        ///W_Проектная зона
        /// </summary>
        public static Guid W_Proektnaya_Zona = new Guid("578d9cfb-ac82-4abf-a961-c0fd4612b7f6");
        /// <summary>
        ///W_Группа конструкций
        /// </summary>
        public static Guid W_Gruppa_Konstruktsiy = new Guid("db30d4fb-5ce5-4a0f-b56c-9ea756d42551");
        /// <summary>
        ///Глубина ниши
        /// </summary>
        public static Guid Glubina_Nishi = new Guid("155517fc-a2f3-4c4d-b870-e37fee2be18e");
        /// <summary>
        ///Поворот УГО
        /// </summary>
        public static Guid Povorot_UGO = new Guid("540f52fc-2a6b-4097-8355-6ba2982e9929");
        /// <summary>
        ///Тип смеситетеля умывальника
        /// </summary>
        public static Guid Tip_Smesitetelya_Umyvalnika = new Guid("09c864fc-e69d-4ac6-ae1f-11eb138b42fd");
        /// <summary>
        ///W_Потолок_Дефекты
        /// </summary>
        public static Guid W_Potolok_Defekty = new Guid("5b7633fd-68df-4247-9ff8-cfc5edbe1a46");
        /// <summary>
        ///Строка 8 должность
        /// </summary>
        public static Guid Stroka_8_Dolzhnost = new Guid("d5f05afd-6b2e-4746-9618-0df31b0465cb");
        /// <summary>
        ///Шифр
        /// </summary>
        public static Guid SHifr = new Guid("2bb330fe-33ae-4ea5-8781-8b3eba10257e");
        /// <summary>
        ///ГК отделка з.п.
        /// </summary>
        public static Guid GK_Otdelka_Zp = new Guid("dd1333fe-85d4-4bb3-80b9-78c42c6543a3");
        /// <summary>
        ///WS_Плинтус_Длина
        ///Экземпляр/Строительство (Параметр проекта)
        /// </summary>
        public static Guid WS_Plintus_Dlina = new Guid("bfe351fe-b1b1-4438-8fc0-d7efe410f216");
        /// <summary>
        ///Кол-во занимаемых адресов
        /// </summary>
        public static Guid Kolvo_Zanimaemykh_Adresov = new Guid("68e780fe-e47e-4d2c-a387-6adf0451bde6");
        /// <summary>
        ///Ширина
        /// </summary>
        public static Guid SHirina_1 = new Guid("5fb082fe-dd46-4a29-b25d-0b2ac20a3dfe");
        /// <summary>
        ///W_Лестницы_Отделка ступеней и площадок_Площадь
        ///Лестницы/Экземпляр/Материалы и отделка (Параметр проекта)
        /// </summary>
        public static Guid W_Lestnitsy_Otdelka_Stupeney_I_Ploshchadok_Ploshchad = new Guid("05819ffe-1639-4b42-adf3-d8ce17a452ed");
        /// <summary>
        ///W_Огнестойкость дверей
        ///Оборудование/Экземпляр/Механизмы (Параметр семейства)
        /// </summary>
        public static Guid W_Ognestoykost_Dverey = new Guid("83f6dbfe-9b4f-4407-aa07-8c91e0dd8a55");
        /// <summary>
        ///Изделие №2
        /// </summary>
        public static Guid Izdelie_2 = new Guid("2bd2effe-fbc1-4338-8f49-43ebb29a7c2a");
        /// <summary>
        ///Условия окружающей среды пространства
        /// </summary>
        public static Guid Usloviya_Okruzhayushchey_Sredy_Prostranstva = new Guid("46a82bff-2aae-4d5e-a599-82a49d491b63");
        /// <summary>
        ///Номинальное напряжение
        /// </summary>
        public static Guid Nominalnoe_Napryazhenie = new Guid("06a55dff-79e9-43a5-a146-6ca55aa98356");
        /// <summary>
        ///№ листа
        /// </summary>
        public static Guid _Lista = new Guid("ea8861ff-bd8c-4525-bf65-d39e1b3a3c7f");
        /// <summary>
        ///10_Дата
        /// </summary>
        public static Guid Data10_ = new Guid("7e86acff-8eb6-4f8c-aacf-4e261a3cea00");
        /// <summary>
        ///W
        /// </summary>
        public static Guid W = new Guid("af96b7ff-de67-4d12-ab42-f16ac786640b");
        /// <summary>
        ///Принадлежность к системам
        ///Параметр принадлежности экземпляра к системе(ам) МГ/ТГ для формирвоания объединённосго параметра "Наименование и техническая характеристика" в спецификации
        /// </summary>
        public static Guid Prinadlezhnost_K_Sistemam = new Guid("2eabffff-a9ca-4cbd-84af-d6b9278bc6c4");
    }
}

