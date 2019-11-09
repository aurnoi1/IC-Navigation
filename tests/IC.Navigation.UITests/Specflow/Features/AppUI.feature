Feature: AppUI
	Ensure the UI is appealing

@mytag
Scenario: Menu page UI is appealing
	Given The application under test has been started
    When I save a picture of "menu page" as "default_menu_page" 
    Then the picture "default_menu_page" should match the approved one

@mytag
Scenario Outline: Pages UI are appealing
	Given The application under test has been started
    When I save a picture of "<page>" as "<picture_name>" 
    Then the picture "<picture_name>" should match the approved one

    Examples: 
    | page      | picture_name      |
    | menu page | default_menu_page |
    | blue page | default_blue_page |
